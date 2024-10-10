using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class SpellGaining : MonoBehaviour
{
    public int callCount = 0;

    public int pushForce = 20;
    public float initialAngularVelocity = 0f;
    public float targetAngularVelocity = 360f; // degrees per second
    public float duration = 2f;
    public Ease ease = Ease.InCubic;

    private float currentAngularVelocity;

    public TMP_InputField inputField;
    public RectTransform targetRect;
    Vector3 target;
    Rigidbody2D rb;
    ChaseMovement chaseMovement;

    bool isAnimating = false;
    private string triggerWords = "";

    private bool doInvisibleNextLevel = false;

    private void OnEnable()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!chaseMovement) chaseMovement = GetComponent<ChaseMovement>();
        inputField.onSubmit.AddListener(OnRegister);
        // convert the target position to world position
        target = Camera.main.ScreenToWorldPoint(targetRect.position);
    }

    void OnRegister(string input)
    {
        if (triggerWords != "") VoiceInputSystem.Instance.Unregister(triggerWords);
        VoiceInputSystem.Instance.Register(input, OnCall);
        triggerWords = input;
    }

    void OnCall(PhraseRecognizedEventArgs args)
    {
        callCount++;
        isAnimating = true;
        switch (callCount)
        {
            case 1:
                transform.DOScale(transform.localScale * 2f, duration).SetEase(ease);
                transform.DOShakePosition(duration).OnComplete(() => isAnimating = false);
                break;
            case 2:
                transform.DOScale(transform.localScale * 1.5f, duration).SetEase(ease);
                transform.DOShakeRotation(duration).OnComplete(() => isAnimating = false);
                break;
            case 3:
                transform.DOScale(0, duration).SetEase(ease);
                //change the rotation to leave the target
                Vector3 direction = (transform.position - target).normalized;
                //alter direction with a random angle
                direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-30, 30)) * direction;
                rb.AddForce(direction * pushForce, ForceMode2D.Impulse);

                DOTween.To(() => initialAngularVelocity, x => currentAngularVelocity = x, targetAngularVelocity,
                        duration)
                    .OnUpdate(() => transform.Rotate(Vector3.forward, currentAngularVelocity * Time.deltaTime))
                    .SetEase(Ease.Linear);

                chaseMovement.enabled = true;
                doInvisibleNextLevel = true;
                chaseMovement.OnEnterRange += () =>
                {
                    chaseMovement.OnEnterRange = null;
                    chaseMovement.enabled = false;
                    gameObject.SetActive(false);
                    if (doInvisibleNextLevel) NextLevel();
                };
                break;
        }
    }

    private void OnBecameInvisible()
    {
        if (doInvisibleNextLevel)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (doInvisibleNextLevel)
        {
            NextLevel();
            doInvisibleNextLevel = false;
        }
    }

    void NextLevel()
    {
        SaveDataManager.Instance.saveData.levelsCompleted
            = Math.Max(SaveDataManager.Instance.saveData.levelsCompleted, PlayerPrefs.GetInt("levelIndex"));
        SceneTransit.Instance.LoadTargetScene("PsyRoom");
    }
}