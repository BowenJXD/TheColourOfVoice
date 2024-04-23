using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Trigger_Type
{
    Trigger_Benj,
    Trigger_Hilda,
    Trigger_Nio,
    Trigger_Ron,
    Trigger_Ave,
    Trigger_Chloe
}

[CreateAssetMenu(fileName = "Node_", menuName = "Event/Message/PlayTimeline")]
public class EN_PlayPreLevelTImeLine : EventNodeBase
{
    public Trigger_Type trigger_Type;
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Trigger_Type:Benj");
        DialogueManager.CloseDialogueBox();
        BigMap.Instance.playTimeline(trigger_Type);
        
        state = NodeState.Finished;
    }
}
