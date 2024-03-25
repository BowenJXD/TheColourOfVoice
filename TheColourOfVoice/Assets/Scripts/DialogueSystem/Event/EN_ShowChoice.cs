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
    public SequenceEventExecutor[] excutors; //每一个分支都是一个顺序执行器

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
        //显示所有选项
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
