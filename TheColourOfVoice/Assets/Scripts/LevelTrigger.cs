using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LevelTrigger : MonoBehaviour
{
    public SequenceEventExecutor testSequenceEventExecutor;
    [SerializeField] private bool isInDialogueArea = false;
    bool isDialogueInit = false;

    void Start()
    {
        
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isInDialogueArea)
        {
            if (!isDialogueInit)
            {
                Debug.Log("Show dialogue");
                isDialogueInit = true;
                if (testSequenceEventExecutor)
                {
                    testSequenceEventExecutor.Init(OnFinishedEvent);
                }

                testSequenceEventExecutor.Excute();
            }

        }
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            isInDialogueArea = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInDialogueArea = false;
        }
    }

    void OnFinishedEvent(bool success)
    {
        Debug.Log(success);
    }
}
