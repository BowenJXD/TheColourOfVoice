using System;
using DG.Tweening;
using UnityEngine;

public class TweenOpacity :  MonoBehaviour, ISetUp
{
    public float interval = 1.0f;
    public float lowerBound = 0.0f;
    public float upperBound = 1.0f;
    public Ease ease = Ease.InOutSine;

    private SpriteRenderer sp;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        if (!sp) sp = GetComponentInChildren<SpriteRenderer>();
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        TweenFade();
    }

    void TweenFade()
    {
        sp?.DOFade(upperBound, interval)
            .SetEase(ease)
            .OnComplete(() => 
                sp?.DOFade(lowerBound, interval)
                    .SetEase(ease)
                    .OnComplete(TweenFade)
            );
    }

}