using DG.Tweening;
using UnityEngine;

/// <summary>
///  DoTween行为节点
/// </summary>
public class DoTweenBehaviour : BehaviourNode
{
    public Ease ease = Ease.Linear;
    public float duration;
    public Tween tween;
    public Transform target;

    public override void Init()
    {
        base.Init();

        if (sequence && sequence.TryGet(BBKey.ENTITY, out Entity bbEntity))
        {
            target = bbEntity.transform;
        }
        if (!target) target = transform;
        if (sequence && sequence.TryGet(BBKey.DURATION, out float bbDuration))
        {
            duration = bbDuration;
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (!skip) UnNext();
        SetUpTween();
        if (!skip) tween.OnComplete(Next);
        tween.Play();
    }

    protected virtual void SetUpTween()
    {
            
    }
}