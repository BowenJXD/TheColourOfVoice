using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState 
{
    Waiting, //待机
    Excuting, //执行中
    Finished //完成
}

public class EventNodeBase : ScriptableObject
{
    protected Action<bool> Onfinished; //回调，bool代表这个节点是否已经完成
    [HideInInspector] public NodeState state;
    public virtual void Init(Action<bool> onFinishedEvent) 
    {
        Onfinished = onFinishedEvent;
        state = NodeState.Waiting;
    }
    
    public virtual void Execute() 
    {
        if(state == NodeState.Excuting) {return; }
        state = NodeState.Excuting;
    }
}
