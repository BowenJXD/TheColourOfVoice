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
        button.gameObject.SetActive(false);
    }
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        //ChooseLevelPanel.Instance.OnPointerEnterCase(transform.parent.gameObject);
        if (slotType == SlotType.LEFT_SLOT && !casePanelIsMoving)
        {
            casePanelIsMoving = true;
            var parent = transform.parent;
            parent.GetComponent<CanvasGroup>().alpha = colorSelectedAlpha;
            RectTransform tempRect = parent.GetComponent<RectTransform>();
            tempRect.DOMove(ChooseLevelPanel.Instance.leftPeekSlot.position,duration).SetEase(Ease.InOutSine).onComplete = () => casePanelIsMoving = false;
            tempRect.DOSizeDelta(ChooseLevelPanel.Instance.leftPeekSlot.sizeDelta,duration).SetEase(Ease.InOutSine);
            tempRect.DOScale(ChooseLevelPanel.Instance.leftPeekSlot.localScale,duration).SetEase(Ease.InOutSine);
            tempRect.DORotate(ChooseLevelPanel.Instance.leftPeekSlot.localEulerAngles,duration).SetEase(Ease.InOutSine);
        }
        else if (slotType == SlotType.RIGHT_SLOT&& !casePanelIsMoving)
        {
            casePanelIsMoving = true;
            transform.parent.GetComponent<CanvasGroup>().alpha = colorSelectedAlpha;
            RectTransform tempRect = GetComponent<RectTransform>();
            tempRect.DOMove(ChooseLevelPanel.Instance.rightPeekSlot.position,duration).SetEase(Ease.InOutSine).onComplete = () => casePanelIsMoving = false;
            tempRect.DOSizeDelta(ChooseLevelPanel.Instance.rightPeekSlot.sizeDelta,duration).SetEase(Ease.InOutSine);
            tempRect.DOScale(ChooseLevelPanel.Instance.rightPeekSlot.localScale,duration).SetEase(Ease.InOutSine);
            tempRect.DORotate(ChooseLevelPanel.Instance.rightPeekSlot.localEulerAngles,duration).SetEase(Ease.InOutSine);
        }

    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        ChooseLevelPanel.Instance.OnpointerExitCase(transform.parent.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer click");
    }
}
