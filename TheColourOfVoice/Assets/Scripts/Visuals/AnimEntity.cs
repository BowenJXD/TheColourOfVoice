using System;
using UnityEngine;

public class AnimEntity : Entity
{
    private BehaviourSequence sequence;

    [HideInInspector] public float animDuration;
    
    public Action onFinish;
    
    Vector3 originScale;
    
    public override void Init()
    {
        if (TryGetComponent(out sequence))
        {
            sequence.Blackboard = new();
            sequence.Set(BBKey.DURATION, animDuration);
            sequence.onFinish += onFinish;
        }
        originScale = transform.localScale;
        base.Init();
    }

    public override void Deinit()
    {
        transform.localScale = originScale;
        
        base.Deinit();
    }
}