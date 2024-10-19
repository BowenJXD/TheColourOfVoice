using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
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
    [Title("关卡完成评分图标")]
    public GameObject caseSettlementIcon;

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
    
    /// <summary>
    /// 生成关卡结算图标
    /// </summary>
    /// <param name="caseIconName"></param>
    public void InitCaseSettlementIcon(string caseIconName)
    {
        //生成Icon的部分
        ResetIcon();
        string iconArtPath = "Arts/CaseSettlementIcon/" + caseIconName;
        Sprite iconSprite = Resources.Load<Sprite>(iconArtPath);
        caseSettlementIcon.GetComponent<Image>().sprite = iconSprite;
        caseSettlementIcon.SetActive(true);
        //动效
        RectTransform rect = caseSettlementIcon.GetComponent<RectTransform>();
        caseSettlementIcon.GetComponent<Image>().DOFade(1, 0.5f);
        rect.DOAnchorPos(new Vector2(494f, -215f), 0.5f);
        rect.DOSizeDelta(new Vector2(267.5f, 267.5f), 0.5f);
    }
   
    /// <summary>
    /// 重置Icon
    /// </summary>
    public void ResetIcon()
    {
        caseSettlementIcon.SetActive(false);
        RectTransform rect = caseSettlementIcon.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(609f, -172f);
        rect.sizeDelta = new Vector2(410f, 410f);
        caseSettlementIcon.GetComponent<Image>().color = new Color(1,1,1,0);
    }

}
