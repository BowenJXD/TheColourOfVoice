using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace BehaviourSequence
{
    /// <summary>
    ///  DoTween移动节点
    /// </summary>
    public class DoTweenMoveBehaviour : DoTweenBehaviour
    {
        public Vector3 moveVector;

        protected override void SetUpTween()
        {
            tween = transform.DOMove(transform.position + moveVector, duration).SetEase(ease);
        }

        protected override void OnFinish()
        {
            base.OnFinish();
        }
    }
}