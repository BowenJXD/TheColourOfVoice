using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
///  DoTween移动节点
/// </summary>
public class DoTweenMoveBehaviour : DoTweenBehaviour
{
    [HideIf("randomLocation")]
    public Vector3 moveVector;
    public bool randomLocation = false;
    [ShowIf("randomLocation")]
    public Vector2 range = new Vector2(0, 0);
    
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
        Vector3 endPos = randomLocation ? new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y)) : target.position + moveVector;
        tween = target.DOMove(endPos, duration).SetEase(ease);
    }
}