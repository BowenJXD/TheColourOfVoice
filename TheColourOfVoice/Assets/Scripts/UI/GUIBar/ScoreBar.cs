using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] Image fillImageScore;
    [SerializeField] SplashGrid splashGrid;
    [SerializeField] float maxScore = 10000f; 
    protected float percentage;
    public float score;
    [SerializeField] Text percentText;

    Canvas canvas;
    void SetScoreText(float score)
    {
        percentText.text = Mathf.RoundToInt(score).ToString();
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
/*        SetPercentText(0);
*/
    }
    
    public Action<float> OnScoreChanged;

    protected virtual void UpdateScore(float newPercentage)
    {
        float addedScore = newPercentage * 100;

        score += addedScore;
        score = Mathf.Min(score, maxScore);
        SetScoreText(score);
        OnScoreChanged?.Invoke(newPercentage);

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

