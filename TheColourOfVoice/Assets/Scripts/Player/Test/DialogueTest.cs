using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    [SerializeField]
    public SequenceEventExecutor testSequenceEventExecutor;
    bool isDialogueInit = false;
    public bool isInDialogueArea = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isInDialogueArea)
        {
            if(!isDialogueInit) 
            {
                Debug.Log("Show dialogue");
                isDialogueInit = true;
                if (testSequenceEventExecutor)
                {
                    testSequenceEventExecutor.Init(OnFinishedEvent);
                }

                testSequenceEventExecutor.Excute();
            }
            else testSequenceEventExecutor.Excute();
        }
       
    }


    void OnFinishedEvent(bool success) 
    {
        Debug.Log(success);
    }
}


