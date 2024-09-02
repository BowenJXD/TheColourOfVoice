using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Would be executed by a <see cref="ConditionBehaviour"/>.
/// </summary>
public abstract class ExecutableBehaviour : MonoBehaviour, ISetUp, IExecutable
{
    [Tooltip("If true, the next executableBehaviour will be executed after 'executing' this one." +
             "If false, the next executableBehaviour will be executed if 'executing' is false.")]
    [HorizontalGroup()] public bool skip = true;
    [ReadOnly] [HorizontalGroup()] public bool executing;

    protected Blackboard blackboard;

    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    /// <summary>
    /// Called by conditionBehaviour
    /// </summary>
    public virtual void Init() { }

    private void OnDisable()
    {
        Deinit();
    }

    public virtual void Deinit() { }

    protected virtual void OnStart() { }

    protected virtual IEnumerator Executing()
    {
        yield return null;
    }
    
    protected virtual void OnFinish() { }
    
    public virtual IEnumerator Execute(Blackboard newExecutor)
    {
        blackboard = newExecutor;
        OnStart();
        yield return Executing();
        if (!skip)
        {
            yield return new WaitUntil(() => !executing);
        }
        OnFinish();
    }
    
    public virtual void StartExe()
    {
        executing = true;
    }
        
    public virtual void FinishExe()
    {
        executing = false;
    }
}