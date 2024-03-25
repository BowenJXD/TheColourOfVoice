using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node_", menuName = "Event/Message/Close DialogueBox")]
public class EN_CloseDialogueBox : EventNodeBase
{
   
    public override void Execute()
    {
        base.Execute();
        DialogueManager.CloseDialogueBox();
        
            state = NodeState.Finished;
        
      
    }
}
