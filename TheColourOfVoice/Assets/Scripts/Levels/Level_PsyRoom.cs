using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Level_PsyRoom : MonoBehaviour
{
    public GameObject player;
    public Camera uiCamera;
    public GameObject mainPanel;
    private int shakeCount = 0;
    private bool playerIsAwake = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsAwake == false)
        {
            Shake(player, 0.1f, 0.1f);
            shakeCount++;

            if (shakeCount == 5)
            {
                LittleWitchAwake();
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

        uiCamera.transform.DOMove(new Vector3(3.31f,0.63f,-10f), 1f).SetEase(Ease.InOutSine)
            .onComplete = OnCameraMoveFinished;

    }

    private void Shake(GameObject target,float powerX, float powerY)
    {
        target.transform.DOShakePosition(1f, new Vector3(powerX, powerY, 0f), 10, 180, false)
            .SetLoops(1, LoopType.Incremental);
    }
    
    private void OnCameraMoveFinished()
    {
        Debug.Log("Camera move finished");
        if (mainPanel)
        {
           
            mainPanel.SetActive(true);
            RectTransform rectTransform = mainPanel.GetComponent<RectTransform>();
            rectTransform.DOAnchorPos(new Vector2(1524.18f, -535.85f), 1f).SetEase(Ease.InOutSine);
            rectTransform.DOSizeDelta(new Vector2(726.76f,964.2f),1f).SetEase(Ease.InOutSine);
        }
    }
}
