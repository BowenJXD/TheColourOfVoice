using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PopUpBubble : MonoBehaviour
{
    [ReadOnly]public TextMeshProUGUI bubbleContent;
    public void ShowBubble(PopUpData data)
    {
        //Debug.Log("ShowBubble");
        bubbleContent.text = data.bubbleText;
        DoFadeBubble(new Vector2(298f, 104f), 1);
    }

    public void Init()
    {
        Debug.Log("dddddddddddddddddddddddddd:" );
        bubbleContent = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(407f,64f);
    }

    public void CloseBubble()
    {
        Debug.Log("CloseBubble");
        DoFadeBubble(new Vector2(407f, 64f), 0);
        bubbleContent.text = "";
    }
    
    private void DoFadeBubble(Vector2 targetPos, float targetAlpha, float fadeDuration = 0.5f, float moveDuration = 0.5f)
    {
        GetComponent<RectTransform>().DOAnchorPos(targetPos, 0.5f);
        GetComponent<CanvasGroup>().DOFade(targetAlpha, 0.5f);
    }
   
}
