using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeTao
{
    /// <summary>
    /// 行为节点，受行为数列控制。拥有一个IEnumerator，用于执行行为。
    /// </summary>
    public abstract class BehaviourNode : MonoBehaviour
    {
        [HideInInspector] public BehaviourSequence sequence;
        [ReadOnly][ShowInInspector] protected bool next = true;

        public virtual void Init()
        {
            
        }
        
        public virtual IEnumerator Execute()
        {
            OnExecute();
            yield return Executing();
            yield return new WaitUntil(() => next);
            OnFinish();
            sequence.Continue(this);
        }
        
        protected virtual void OnExecute()
        {
            
        }

        protected virtual IEnumerator Executing()
        {
            yield return null;
        }
        
        protected virtual void OnFinish()
        {
            
        }
        
        public virtual void UnNext()
        {
            next = false;
        }
        
        public virtual void Next()
        {
            next = true;
        }
    }
}