using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType
{
    LEFT_SLOT,
    RIGHT_SLOT,
    CURRENT_SLOT,
    HIDDEN  
}

public class PatientCase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public Button button;
    public int id;  
    public SlotType slotType;
    private bool casePanelIsMoving = false;
    //数据
    public TextMeshProUGUI patientName;
    public TextMeshProUGUI patientAge;
    public TextMeshProUGUI patientSymptoms;
    public TextMeshProUGUI patientDescription;
    public Image patientImage;
    public LevelState levelState;
    public float colorSelectedAlpha = 0.7f;
    public float colorAlpha = 0.4f;
    public float duration = 0.2f;


    public void InstantiateCase(CaseData caseData,SlotType patientSlotType)
    {
        patientName.text = caseData.patientName;
        patientAge.text = caseData.patientAge;
        patientSymptoms.text = caseData.patintSymptoms;
        patientDescription.text = caseData.patientDescription;
        patientImage.sprite = caseData.patientImage;
        this.slotType = patientSlotType;
        levelState = caseData.levelState;
        button.gameObject.SetActive(false);
    }
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        ChooseLevelPanel.Instance.OnPointerEnterCase(transform.parent.gameObject);
       
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        ChooseLevelPanel.Instance.OnpointerExitCase(transform.parent.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer click");
        ChooseLevelPanel.Instance.OnPointerClickCase(transform.parent.gameObject);
    }
}
