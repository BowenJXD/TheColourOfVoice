using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Level_PsyRoom : Singleton<Level_PsyRoom>
{
    public GameObject player;
    public Camera uiCamera;
    public GameObject mainPanel;
    private int shakeCount = 0;
    private bool playerIsAwake = false;
    [SerializeField] private Light2D sightLight;
    [SerializeField] private Light2D sunShaftlight;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsAwake == false)
        {
            Shake(player, 0.1f, 0.1f);
            shakeCount++;

            if (shakeCount == 5)
            {
                LittleWitchAwake(); //小魔女苏醒
            }
        }
    }

    private void LittleWitchAwake()
    {
        playerIsAwake = true;
        if (!uiCamera)
        {
            return;
        }
        
        //相机移动
        uiCamera.transform.DOMove(new Vector3(0,-0.71f,-10f), 1f).SetEase(Ease.InOutSine)
            .onComplete = OnCameraMoveFinished;
        
        //开启光源
        float awakeProcessValue = 0f;
        DOTween.To(() => awakeProcessValue, x => awakeProcessValue = x, 1f, 1f)
            .onUpdate = () =>
        {
            sightLight.pointLightOuterRadius = Mathf.Lerp(2.69f, 8f, awakeProcessValue);
            sunShaftlight.intensity = Mathf.Lerp(0f, 10f, awakeProcessValue);
        };
    }

    private void Shake(GameObject target,float powerX, float powerY)
    {
        target.transform.DOShakePosition(1f, new Vector3(powerX, powerY, 0f), 10, 180, false)
            .SetLoops(1, LoopType.Incremental);
    }
    
    /// <summary>
    /// Callback：当相机移动结束时调用
    /// </summary>
    private void OnCameraMoveFinished()
    {
        Debug.Log("Camera move finished");
        if (mainPanel)
        {
           
            mainPanel.SetActive(true);
            RectTransform rectTransform = mainPanel.GetComponent<RectTransform>();
            rectTransform.DOAnchorPos(new Vector2(960f, -884f), 1f).SetEase(Ease.InOutSine);
            rectTransform.DOSizeDelta(new Vector2(1170f,316.37f),1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                //TODO:显示文字
                Debug.Log("Panel move finished, show text");
            });
        }
    }

    /// <summary>
    /// Callback：当小女巫醒来时调用
    /// </summary>
    private void OnLittleWitchAwake()
    {
        Debug.Log("Little witch is awake");
    }
}
