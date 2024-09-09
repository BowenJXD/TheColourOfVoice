using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PopUpBubble : MonoBehaviour
{
    public TextMeshProUGUI bubbleContent;
    public GameObject characterPortrait;
    [ReadOnly]public string currentCase;
    public List<Sprite> bubbleSprites;
    private Dictionary<string, Sprite> bubbleSpriteDict;
    public void ShowBubble(PopUpData data)
    {
        try
        {
            //Debug.Log(data.characterName);
            bubbleContent.text = data.bubbleText;
            currentCase = splitString(data.characterName.ToString());
            //Debug.Log($"currentCase: {currentCase}");
            characterPortrait.TryGetComponent<Image>(out var protrait);
            Debug.Log(bubbleSpriteDict[currentCase].name);
            protrait.sprite = bubbleSpriteDict[currentCase];
            DoFadeBubble(new Vector2(298f, 104f), 1);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
       
    }

    public void Init()
    {
        bubbleSpriteDict = new Dictionary<string, Sprite>();
        //bubbleSprites = new List<GameObject>();
        bubbleSprites = (Resources.LoadAll<Sprite>("Arts/UI_CasesImage")).ToList();
        if (bubbleSprites.Count == 0)
        {
            Debug.Log("Failed to load bubble sprites from Resources.");
            return;
        }
        
        foreach(Sprite bubbleSprite in bubbleSprites)
        {
            bubbleSpriteDict.Add(bubbleSprite.name,bubbleSprite);
            //Debug.Log($"Added {bubbleSprite.name} to the dictionary");
        }
        //bubbleContent = GetComponentInChildren<TextMeshProUGUI>();
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
   
    private string splitString(string str)
    {
        //Debug.Log($"Splitting string: {str}");
        string[] split = str.Split('_');
        if (split.Length > 1)
        {
            string result = split[1]; // 取得 _ 后面的部分
            Debug.Log($"Split result: {result}");
            return result;
        }
        return str;
    }
}
