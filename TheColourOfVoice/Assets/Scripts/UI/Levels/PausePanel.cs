using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] GameObject PauseUI;

    private void Awake()
    {
        LeanTween.reset();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*if (Time.timeScale == 0 && !PauseUI.gameObject.activeSelf) return;*/ // prevent opening the spell panel when the game is paused
            if (!PauseUI.gameObject.activeSelf)
            {
                //Debug.Log("PausePanel");
                Time.timeScale = 0;
                PauseUI.gameObject.SetActive(true);
                LeanTween.moveLocal(PauseUI, new Vector3(0f, -11f, 0f), 1f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInOutBack);
                
            }
            else
            {
                LeanTween.moveLocal(PauseUI, new Vector3(0f, 400f, 0f), 1f).setIgnoreTimeScale(true).setOnComplete(() =>
                    {
                        PauseUI.gameObject.SetActive(false);
                        Time.timeScale = 1; 
                    });

            }
            
        }
    }
}
