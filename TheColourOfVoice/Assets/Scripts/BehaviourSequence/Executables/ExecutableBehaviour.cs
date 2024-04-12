using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Would be executed by a <see cref="ConditionBehaviour"/>.
/// </summary>
public abstract class ExecutableBehaviour : MonoBehaviour, ISetUp, IExecutable
{
    [Tooltip("If true, the next executableBehaviour will be executed after 'executing' this one." +
             "If false, the next executableBehaviour will be executed if 'next' is true.")]
    [HorizontalGroup()] public bool skip;
    [HorizontalGroup()] public bool next; 

    protected IExecutor executor;

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
    
    public virtual IEnumerator Execute(IExecutor newExecutor)
    {
        executor = newExecutor;
        OnStart();
        yield return Executing();
        if (!skip)
        {
            yield return new WaitUntil(() => next);
            UnNext();
        }
        OnFinish();
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