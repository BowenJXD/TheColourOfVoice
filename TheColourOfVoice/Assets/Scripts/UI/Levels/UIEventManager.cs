using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] GameObject PauseUI,StarUI;
    private bool isUImoving = false;
    private void Awake()
    {
        LeanTween.reset();
    }
    public void Resume()
    {
        PausePanelEnd();

    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*if (Time.timeScale == 0 && !PauseUI.gameObject.activeSelf) return;*/ // prevent opening the spell panel when the game is paused
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
        //LeanTween.moveLocal(PauseUI, new Vector3(0f, -20f, 0f), 1f).setIgnoreTimeScale(true);
        PauseUI.TryGetComponent(out RectTransform rectTransform);
        //rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.DOAnchorPos(new Vector2(0,0),1f).SetEase(Ease.OutBounce).onComplete = () =>
        {
            
            isUImoving = false;
            StarUI.SetActive(true);
        };
    }

    void PausePanelEnd()
    {
        isUImoving = true;
        /*LeanTween.moveLocal(PauseUI, new Vector3(0f, 400f, 0f), 1f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            PauseUI.gameObject.SetActive(false);
            Time.timeScale = 1; 
        });*/
        PauseUI.TryGetComponent(out RectTransform rectTransform);
        rectTransform.DOAnchorPos(new Vector2(0,400),1f).SetEase(Ease.OutBounce).onComplete = () =>
        {
            PauseUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            isUImoving = false;
        };
    }
}
