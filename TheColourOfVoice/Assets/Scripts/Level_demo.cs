using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_demo : MonoBehaviour
{
    [SerializeField] public SequenceEventExecutor week6SequenceEventExcutor;
    [SerializeField] private GameObject player;
    public bool skip = false;
    public int totalTime = 180;
    public TextMeshProUGUI text;
    [SerializeField] public GameObject UICanvas;
    private GameObject playerUI;
    private GameObject endLevelUI;
    [SerializeField] public GameObject StarController;
    private int index;
    private GameObject[] selectedSpell; // 引用选定的法术对象

    private void Awake()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<Fire>().enabled = false;
        Time.timeScale = 0;
        if (!skip && week6SequenceEventExcutor)
        {
            week6SequenceEventExcutor.Init(OnFinishedEvent);
            Debug.Log("Excute sequence");
            week6SequenceEventExcutor.Excute();
        }
        
        if (UICanvas != null)
        {
            playerUI = UICanvas.transform.Find("PlayerUI").gameObject;
            endLevelUI = UICanvas.transform.Find("EndLevelUI").gameObject;
            /*playerUI.SetActive(true);
            endLevelUI.SetActive(false);*/
        }
        if (!player)
        {
            return;
        }
        selectedSpell = new GameObject[3];
        if(player!=null)
        {
            Transform spellManagerTransform = player.transform.Find("SpellManager/Spells");
            if(selectedSpell!=null)
            {
                /*selectedSpell[0] = spellManagerTransform.Find("The Glowing Confidence").gameObject;
                selectedSpell[1] = spellManagerTransform.Find("The Burning Passion").gameObject;
                selectedSpell[2] = spellManagerTransform.Find("The Calming Laser").gameObject;*/

            }
        }
        
        if (skip)
        {
            Debug.Log("Skipdialogue");
            OnFinishedEvent(false);
            return;
        }

        
       

        if(index== 1)
        {
            if (selectedSpell[1] != null)
            {
                selectedSpell[1].SetActive(false);
            }
            if (selectedSpell[2] != null)
            {
                selectedSpell[2].SetActive(false);
            }
        }
        if(index== 2)
        {
            if (selectedSpell[1] != null)
            {
                selectedSpell[1].SetActive(true);
            }
            if (selectedSpell[2] != null)
            {
                selectedSpell[2].SetActive(false);
            }
        }
        if(index== 3)
        {
            if (selectedSpell[1] != null)
            {
                selectedSpell[1].SetActive(true);
            }
            if (selectedSpell[2] != null)
            {
                selectedSpell[2].SetActive(true);
            }
        }
        
       
    }

    void OnFinishedEvent(bool success)
    {
        Debug.Log(success);
        StartCoroutine(Timer());
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponentInChildren<Fire>().enabled = true;
        Time.timeScale = 1;
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
        if (totalTime <= 0)
        {
            ShowEndLevelUI();

        }

    }
    
    void ShowEndLevelUI()
    {
        StarController.GetComponent<StarController>().UpdateStars();
        if (playerUI != null)
        {
            playerUI.SetActive(false); 
        }

        if (endLevelUI != null)
        {
            endLevelUI.SetActive(true); 
        }

    }
    
    public void ResetTimer()
    {
        totalTime = 180; 
        Time.timeScale = 1; 
    }
}