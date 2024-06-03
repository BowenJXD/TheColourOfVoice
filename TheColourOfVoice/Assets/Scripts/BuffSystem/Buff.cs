using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Pool;

public enum FMODEffectState
{
    None,
    Start,
    End,
}

public class Buff : MonoBehaviour, ISetUp
{
    public float duration;
    public bool isStackable;
    BuffOwner owner;
    LoopTask loopTask;
    
    [HideInInspector] public ObjectPool<Buff> buffPool;
    StudioEventEmitter emitter;

    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        emitter = GetComponent<StudioEventEmitter>();
        Init();
    }

    public virtual void Init()
    {
        if (buffPool == null) buffPool = new ObjectPool<Buff>(() => Instantiate(this), buff => buff.gameObject.SetActive(true),
            buff => buff.gameObject.SetActive(false), buff => Destroy(buff.gameObject));
        if (duration > 0) loopTask = new LoopTask{interval = duration, finishAction = () =>
        {
            Remove();
        }};
    }

    public void Remove()
    {
        if (owner) owner.RemoveBuff(this);
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    public virtual bool CanApply(BuffOwner buffOwner)
    {
        return true;
    }

    public virtual void OnApply(BuffOwner buffOwner)
    {
        owner = buffOwner;
        if (emitter) emitter.SetParameter("EffectState", (int)FMODEffectState.Start);
        loopTask?.Start();
    }

    public virtual void OnStack()
    {
        
    }

    public virtual void OnRemove()
    {
        buffPool.Release(this);
        loopTask?.Stop();
        if (emitter) emitter.SetParameter("EffectState", (int)FMODEffectState.End);
    }
}