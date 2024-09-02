using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
///  DoTween移动节点
/// </summary>
public class DoTweenMoveBehaviour : DoTweenBehaviour
{
    public Vector3 moveVector;

    protected override void OnStart()
    {
        if (blackboard.TryGet(BBKey.TARGET, out Transform target))
        {
            moveVector = target.position - this.target.position;
        }
        base.OnStart();
    }

    protected override void SetUpTween()
    {
        tween = target.DOMove(target.position + moveVector, duration).SetEase(ease);
    }
}