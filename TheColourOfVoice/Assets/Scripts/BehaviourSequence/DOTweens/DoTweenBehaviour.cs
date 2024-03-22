using DG.Tweening;
using UnityEngine;

/// <summary>
///  DoTween行为节点
/// </summary>
public class DoTweenBehaviour : BehaviourNode
{
    [Tooltip("If true, the next sequence will be executed when this sequence finishes. " +
             "If false, the next sequence will be executed after this sequence starts executing.")]
    public bool nextOnFinish = true;
    public Ease ease = Ease.Linear;
    public float duration;
    public Tween tween;
    public Transform target;

    public override void Init()
    {
        base.Init();

        if (sequence.TryGet(BBKey.ENTITY, out Entity bbEntity))
        {
            target = bbEntity.transform;
        }
        if (!target) target = transform;
        if (sequence.TryGet(BBKey.DURATION, out float bbDuration))
        {
            duration = bbDuration;
        }
    }

    protected override void OnExecute()
    {
        base.OnExecute();
        if (nextOnFinish) UnNext();
        SetUpTween();
        if (nextOnFinish) tween.OnComplete(Next);
        tween.Play();
    }

    protected virtual void SetUpTween()
    {
            
    }
}