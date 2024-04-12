using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 行为节点，受行为数列控制。拥有一个IEnumerator，用于执行行为。
/// </summary>
public abstract class BehaviourNode : ExecutableBehaviour
{
    [HideInInspector] public BehaviourSequence sequence;

    public override IEnumerator Execute(IExecutor newExecutor)
    {
        yield return base.Execute(newExecutor);
        if (sequence) sequence.Continue(this);
    }
}