using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaperButton : MonoBehaviour
{
    //打开选关面板的部分
    [SerializeField] private GameObject chooseLevelPanel;
    [SerializeField] private GameObject paperOutline;
    [SerializeField] private SpriteRenderer paperSpriteImage;
    private void Start()
    {
        //chooseLevelPanel = GameObject.Find("ChooseLevelPanel");
        paperSpriteImage = GetComponent<SpriteRenderer>();
        paperOutline = transform.GetChild(0).gameObject;
        if (chooseLevelPanel == null || paperSpriteImage == null || paperOutline == null)
        {
            return;
        }
        
        chooseLevelPanel.SetActive(false);
    }

    

    private void OnMouseDown()
    {
        
    }
    
    private void OnMouseEnter()
    {
        paperOutline.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        paperOutline.SetActive(false);
    }
    private void OnMouseUp()
    {
        chooseLevelPanel.SetActive(true);
       
    }
}
