using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMap : MonoBehaviour
{
    [Header("Dialogue for Benj")]
    public SequenceEventExecutor benj_preSequence;

    private void Start()
    {
        
    }
    public void StartDialogueSequence(SequenceEventExecutor sequenceEventExecutor) 
    {
        Debug.Log("Start Benji Dialogue");
        if (sequenceEventExecutor == null) 
        {
            Debug.Log("Failure to excute Benji Dialogue");
            return;
        }

        sequenceEventExecutor.Init(OnFinishedEvent);
        sequenceEventExecutor.Excute();
    }


    void OnFinishedEvent(bool success)
    {
        Debug.Log(success);
    }
}
