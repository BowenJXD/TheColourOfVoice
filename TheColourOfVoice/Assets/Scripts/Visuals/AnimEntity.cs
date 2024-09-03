using System;
using UnityEngine;

public class AnimEntity : Entity
{
    private BehaviourSequence sequence;

    [HideInInspector] public float animDuration;
    
    public bool deinitOnAnimEnd = false;
    
    public Action onFinish;
    
    Vector3 originScale;
    
    public override void Init()
    {
        base.Init();
        if (TryGetComponent(out Animator ani))
        {
            animDuration = ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            if (deinitOnAnimEnd)
            {
                Invoke(nameof(Deinit), animDuration);
            }
        }
        if (TryGetComponent(out sequence))
        {
            sequence.blackboard = new();
            sequence.Set(BBKey.DURATION, animDuration);
            sequence.onFinish += onFinish;
        }
        originScale = transform.localScale;
    }

    public override void Deinit()
    {
        transform.localScale = originScale;
        
        base.Deinit();
    }
}