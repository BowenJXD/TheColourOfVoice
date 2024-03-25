using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ChoiceData 
{
    public string content;
    public bool bQuickLocate;
}

[CreateAssetMenu(fileName = "Node_", menuName = "Event/Message/Show Choices")]
public class EN_ShowChoice : EventNodeBase
{
    public int defaultSelectIndex = 0;
    public ChoiceData[] datas;
    public SequenceEventExecutor[] excutors; //ÿһ����֧����һ��˳��ִ����

    public override void Init(Action<bool> onFinishedEvent)
    {
        base.Init(onFinishedEvent);
        foreach (var excutor in excutors) 
        {
            excutor.Init(Onfinished);
        }
    }

    public override void Execute()
    {
        base.Execute();
        //��ʾ����ѡ��
        DialogueManager.CreateDialogueChocies(datas, OnChoiceConfirm, defaultSelectIndex);

    }

    private void OnChoiceConfirm(int index) 
    {
        if (index < excutors.Length && null != excutors[index])
        {
            excutors[index].Excute();
        }
        else
        {
            Onfinished(true);
        }
    }
}
