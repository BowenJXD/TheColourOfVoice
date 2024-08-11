using System.Collections;
using System.Collections.Generic;
using TMPro;
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

public class PatientCase : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private Button button;
    public SlotType slotType;
    
    //数据
    public TextMeshProUGUI patientName;
    public TextMeshProUGUI patientAge;
    public TextMeshProUGUI patientSymptoms;
    public TextMeshProUGUI patientDescription;
    public Image patientImage;

    public void InstantiateCase(CaseData caseData,SlotType slotType)
    {
        patientName.text = caseData.patientName;
        patientAge.text = caseData.patientAge;
        patientSymptoms.text = caseData.patintSymptoms;
        patientDescription.text = caseData.patientDescription;
        patientImage.sprite = caseData.patientImage;
        this.slotType = slotType;
    }
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
    }
}
