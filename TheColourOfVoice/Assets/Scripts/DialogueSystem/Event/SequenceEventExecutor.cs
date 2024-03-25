using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Executor_", menuName = "Event/Sequence Excutor")]
public class SequenceEventExecutor : ScriptableObject
{
    public Action<bool> OnFinished; //bool参数代表执行器执行是否成功

    private int _index; //执行到第几个节点
    public EventNodeBase[] nodes;
    public void Init(Action<bool> onFinishEvent) 
    {
        _index = 0;
        foreach (EventNodeBase item in nodes) 
        {
            item.Init(OnNodeFinished); //节点的回调函数和Init函数
        }

        OnFinished = onFinishEvent; //执行器的回调函数
    }

    /// <summary>
    /// 判断一个节点是否成功完成，如果完成则调用这个回调函数
    /// </summary>
    /// <param name="bIsNodeSuccess"></param>
    private void OnNodeFinished(bool bIsNodeSuccess) 
    {
        if (bIsNodeSuccess)
        {
            ExcuteNextNode();
        }
        else
        {
            OnFinished(false);
        }
    }


    private void ExcuteNextNode() 
    {
        if (_index < nodes.Length)
        {
            if (nodes[_index].state == NodeState.Waiting)
            {
                nodes[_index].Execute();
                _index++;
            }
            
        }
        else
        {
            OnFinished(true);
        }
    }


    public void Excute() 
    {
        _index = 0;
        ExcuteNextNode();
    }
}
