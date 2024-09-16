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
    [HideIf("randomLocation")]
    public float speed = 1;
    public bool randomLocation = false;
    [ShowIf("randomLocation")]
    public Vector2 range = new Vector2(0, 0);

    public Animator ani;

    protected override void OnStart()
    {
        if (blackboard.TryGet(BBKey.TARGET, out Transform target))
        {
            moveVector = (target.position - this.target.position).normalized;
        }
        base.OnStart();
    }

    protected override void SetUpTween()
    {
        Vector3 endPos = randomLocation 
            ? new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y)) 
            : target.position + moveVector * speed;
        tween = DOTween.Sequence().Append(target.DOMove(endPos, duration).SetEase(ease));
        if (ani)
        {
            ani.SetFloat("moveX", moveVector.x);
            ani.SetFloat("moveY", moveVector.y);
            ani.SetBool("isMoving", true);
        }
    }

    public override void FinishExe()
    {
        base.FinishExe();
        if (ani)
        {
            ani.SetBool("isMoving", false);
        }
    }
}