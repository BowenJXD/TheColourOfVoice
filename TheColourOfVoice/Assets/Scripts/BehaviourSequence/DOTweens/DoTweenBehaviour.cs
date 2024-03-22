using DG.Tweening;
using UnityEngine;

namespace BehaviourSequence
{
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
}