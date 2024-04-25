using System;
using UnityEngine;
using UnityEngine.Pool;

public class Buff : MonoBehaviour, ISetUp
{
    public float duration;
    public bool isStackable;
    BuffOwner owner;
    LoopTask loopTask;
    
    public ObjectPool<Buff> buffPool;

    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        Init();
    }

    public virtual void Init()
    {
        if (buffPool == null) buffPool = new ObjectPool<Buff>(() => Instantiate(this), buff => buff.gameObject.SetActive(true),
            buff => buff.gameObject.SetActive(false), buff => Destroy(buff.gameObject));
        if (loopTask == null) loopTask = new LoopTask{interval = duration, finishAction = () => buffPool.Release(this)};
    }

    public void Remove()
    {
        owner.RemoveBuff(this);
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }

    public virtual void OnApply(BuffOwner buffOwner)
    {
        owner = buffOwner;
        loopTask.Start();
    }

    public virtual void OnStack()
    {
        
    }

    public virtual void OnRemove()
    {
        loopTask.Stop();
    }

    private void OnDisable()
    {
        Remove();
    }
}