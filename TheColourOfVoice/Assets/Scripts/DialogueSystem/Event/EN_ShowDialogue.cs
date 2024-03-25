using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueData   
{
    public string speaker;
    [Multiline] public string content;
    public bool AutoNext;
    public bool NeedTyping;
    public bool CanQuickShow;
}

[CreateAssetMenu(fileName = "Node_", menuName = "Event/Message/Show Dialogue")]
public class EN_ShowDialogue : EventNodeBase
{
    public DialogueData[] datas;
    public int boxStyle = 0;
    private int _index;

    public override void Execute()
    {
        base.Execute();
        _index = 0;
        DialogueManager.OpenDialogueBox(ShowNextDialogue, boxStyle);
    }

    private void ShowNextDialogue(bool forceDisplayDirectly) 
    {
        if (_index < datas.Length)
        {
            DialogueData data = new DialogueData() 
            {
                speaker = datas[_index].speaker,
                content = datas[_index].content,
                CanQuickShow = datas[_index].CanQuickShow,
                AutoNext = datas[_index].AutoNext,
                NeedTyping = !forceDisplayDirectly && datas[_index].NeedTyping
            };
            DialogueManager.PrintDialogue(data);
            _index++;
        }
        else 
        {
            state = NodeState.Finished;
            Onfinished(true); 
        } 
           
    }
}
