using DG.Tweening;
using UnityEngine;

public class DoTweenScaleBehaviour : DoTweenBehaviour
{
    public float scaleSpeed;
    public float maxScale;

    protected override void SetUpTween()
    {
        float finalScale = Mathf.Max(duration * scaleSpeed, maxScale);
        tween = target.DOScale(finalScale, duration).SetEase(ease);
    }
}