using DG.Tweening;
using UnityEngine;

public class DoFadeExe : DoTweenBehaviour
{
    public SpriteRenderer sp;
    
    public float fadeTo;

    public override void Init()
    {
        base.Init();
        if (!sp) sp = target.GetComponent<SpriteRenderer>();
    }

    protected override void SetUpTween()
    {
        base.SetUpTween();
        tween.Append(sp.DOFade(fadeTo, duration).SetEase(ease));
    }
}