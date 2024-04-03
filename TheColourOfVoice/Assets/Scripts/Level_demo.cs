using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_demo : MonoBehaviour
{
    [SerializeField]
    public SequenceEventExecutor week6SequenceEventExcutor;
    [SerializeField] private GameObject player;
    bool isDialogueInit = false;
    void Start()
    {
        if (!player)
        {
            return;
        }

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<Fire>().enabled = false;
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
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Fire>().enabled = true;
        TextPainter.Instance.PaintText();
    }
}
