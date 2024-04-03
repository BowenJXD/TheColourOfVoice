using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_demo : MonoBehaviour
{
    [SerializeField]
    public SequenceEventExecutor week6SequenceEventExcutor;
    bool isDialogueInit = false;
    void Start()
    {     
            if (week6SequenceEventExcutor)
            {
            week6SequenceEventExcutor.Init(OnFinishedEvent);
            }

        week6SequenceEventExcutor.Excute();
        
    }

    void OnFinishedEvent(bool success)
    {
        Debug.Log(success);
        EnemyGenerator.Instance.NewTask();
    }
}
