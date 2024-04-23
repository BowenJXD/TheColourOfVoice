using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Executor_", menuName = "Event/Sequence Excutor")]
public class SequenceEventExecutor : ScriptableObject
{
    public Action<bool> OnFinished; //bool��������ִ����ִ���Ƿ�ɹ�

    private int _index; //ִ�е��ڼ����ڵ�
    public EventNodeBase[] nodes;
    public void Init(Action<bool> onFinishEvent) 
    {
        _index = 0;
        foreach (EventNodeBase item in nodes) 
        {
            item.Init(OnNodeFinished); //�ڵ�Ļص�������Init����
        }

        OnFinished = onFinishEvent; //ִ�����Ļص�����
    }

    /// <summary>
    /// �ж�һ���ڵ��Ƿ�ɹ���ɣ����������������ص�����
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

        if (_index >= nodes.Length)
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
