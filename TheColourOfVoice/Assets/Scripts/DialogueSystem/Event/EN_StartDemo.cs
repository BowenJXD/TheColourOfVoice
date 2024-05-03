using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Node_", menuName = "Event/Message/StartDemo")]
public class EN_StartDemo : EventNodeBase
{

    public override void Execute()
    {
        base.Execute();
        DialogueManager.CloseDialogueBox();
        LevelManager.Instance.Init();
        state = NodeState.Finished;


    }
}
