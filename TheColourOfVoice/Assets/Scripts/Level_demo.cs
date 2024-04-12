using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level_demo : MonoBehaviour
{
    [SerializeField] public SequenceEventExecutor week6SequenceEventExcutor;
    [SerializeField] private GameObject player;
    public bool skip = false;
    public int totalTime = 60;
    public TextMeshProUGUI text;

    void Start()
    {
        if (!player)
        {
            return;
        }

        if (skip)
        {
            OnFinishedEvent(false);
            return;
        }

        
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<Fire>().enabled = false;
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
        StartCoroutine(Timer());
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponentInChildren<Fire>().enabled = true;
        TextPainter.Instance.PaintText();
    }

    IEnumerator Timer()
    {
        while (totalTime >= 0)
        {
            text.GetComponent<TextMeshProUGUI>().text = totalTime.ToString();
            yield return new WaitForSeconds(1);
            totalTime--;
        }

        Debug.Log("Time's up!");
        Time.timeScale = 0;
        LogUtil.Instance.LogCSV();
    }
}