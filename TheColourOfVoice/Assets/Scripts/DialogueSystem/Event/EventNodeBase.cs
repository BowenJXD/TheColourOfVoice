using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState 
{
    Waiting, //����
    Excuting, //ִ����
    Finished //���
}

public class EventNodeBase : ScriptableObject
{
    protected Action<bool> Onfinished; //�ص���bool��������ڵ��Ƿ��Ѿ����
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
