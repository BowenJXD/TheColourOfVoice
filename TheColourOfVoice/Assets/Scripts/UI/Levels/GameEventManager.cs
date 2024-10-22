using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] GameObject PauseUI, StarUI;
    [SerializeField] ParticleSystem myParticleSystem;
    [SerializeField] private GameObject StarController;

    private bool isUImoving = false;

    private void Awake()
    {
        LeanTween.reset();
        PauseUI.SetActive(false);
    }

    public void Resume()
    {
        PausePanelEnd();
    }

    public void Quit()
    {
        SceneManager.LoadScene("PsyRoom");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // prevent opening the spell panel when the game is paused
            if (!PauseUI.gameObject.activeSelf && !isUImoving)
            {
                Debug.Log("PausePanel");
                PausePanelStart();
            }
            else if (!isUImoving)
            {
                Debug.Log("PausePanelEnd");
                PausePanelEnd();
            }
        }

    }


    void PausePanelStart()
    {
        isUImoving = true;
        Debug.Log("PausePanel");
        Time.timeScale = 0;
        PauseUI.gameObject.SetActive(true);
        LeanTween.moveLocal(PauseUI, new Vector3(0f, -20f, 0f), 1f).setIgnoreTimeScale(true)
            .setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
            {
                isUImoving = false;
                StarUI.SetActive(true);

            });
        PauseUI.TryGetComponent(out RectTransform rectTransform);

    }

    void PausePanelEnd()
    {
        isUImoving = true;
        LeanTween.moveLocal(PauseUI, new Vector3(0f, 400f, 0f), 1f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            PauseUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            isUImoving = false;
            StarUI.SetActive(false);

        });
        PauseUI.TryGetComponent(out RectTransform rectTransform);

    }


}
