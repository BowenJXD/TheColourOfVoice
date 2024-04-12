using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class DoTweenScale1Behaviour : DoTweenBehaviour
{
    public float scaleSpeed;
    public float scaleLimit;

    protected override void SetUpTween()
    {
        float originScale = target.localScale.z;
        float targetScale = originScale + duration * scaleSpeed;
        float finalScale = scaleSpeed > 0 ? Mathf.Min(targetScale, scaleLimit) : Mathf.Max(targetScale, scaleLimit);
        tween = target.DOScale(finalScale, duration).SetEase(ease);
    }
}