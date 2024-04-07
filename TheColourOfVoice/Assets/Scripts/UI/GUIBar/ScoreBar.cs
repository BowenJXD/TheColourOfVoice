using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] Image fillImageScore;
    [SerializeField] SplashGrid splashGrid;
    [SerializeField] float maxScore = 1f; 
    protected float percentage;
    float score;
    [SerializeField] Text percentText;

    Canvas canvas;
    void SetPercentText()
    {
        percentText.text = Mathf.RoundToInt(percentage * 10000f).ToString();
    }

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        UpdateScore(0);
        StartCoroutine(UpdatePercentageEverySecond());
    }

    IEnumerator UpdatePercentageEverySecond()
    {
        while (true) 
        {
            yield return new WaitForSeconds(1f); 
            Initialize(splashGrid); 
        }
    }

    protected virtual void Initialize(SplashGrid splashGrid)
    {
        percentage = splashGrid.paintedPercentage;
        UpdateScore(percentage);
        SetPercentText();

    }


    protected virtual void UpdateScore(float newPercentage)
    {
        float addedScore = newPercentage * maxScore;

        score += addedScore;
        score = Mathf.Min(score, maxScore);
        SetPercentText();

        UpdateUI();
    }

    void UpdateUI()
    {
        if (fillImageScore != null)
        {
            fillImageScore.fillAmount = score / maxScore;
            //Debug.Log("score");
        }
    }
}

