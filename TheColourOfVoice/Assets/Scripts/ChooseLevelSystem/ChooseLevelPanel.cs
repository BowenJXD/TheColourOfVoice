using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



public class ChooseLevelPanel : Singleton<ChooseLevelPanel>
{
    //各个Case的数据
    public List<CaseData> caseDataList;
    public List<GameObject> currentCaseList; //当前可以显示的所有Case
    //记载各个Case生成的位置
    public RectTransform leftslotPos;
    public RectTransform rightslotPos;
    public RectTransform leftPeekSlot;
    public RectTransform rightPeekSlot;
    public RectTransform currentSlotPos;
    public float colorSelectedAlpha = 0.7f;
    public float colorAlpha = 0.4f;
    public float duration = 0.2f;
    //private bool casePanelIsMoving = false;
    
    //循环结构的参数
    private int currentIndex = 0;
    private void Awake()
    {
        foreach (var caseData in Resources.LoadAll<CaseData>("CaseData"))
        {
            caseDataList.Add(caseData);
        }
    }

    private void Start()
    {
        foreach (var caseData in caseDataList)
        {
            Debug.Log("CaseData: "+caseData.patientName);
            InstantiateCase(currentSlotPos,caseData,SlotType.CURRENT_SLOT);
        }

        for (int i = 0; i < currentCaseList.Count; i++)
        {
            currentCaseList[i].GetComponentInChildren<PatientCase>().id = i;
        }

        MoveCase(currentCaseList[0],currentSlotPos,1,SlotType.CURRENT_SLOT,true);
        MoveCase(currentCaseList[^1],leftslotPos,colorAlpha,SlotType.LEFT_SLOT);
        MoveCase(currentCaseList[1],rightslotPos,colorAlpha,SlotType.RIGHT_SLOT);
    }
    
    void InstantiateCase(RectTransform rectTransform,CaseData caseData,SlotType slotType)
    {
        GameObject tempCaseObject = Instantiate(Resources.Load<GameObject>("Prefabs/CaseTest"),transform);
        RectTransform tempCaseRect = tempCaseObject.transform.parent.GetComponent<RectTransform>();
        tempCaseObject.transform.position = rectTransform.position;
        tempCaseObject.GetComponentInChildren<PatientCase>().InstantiateCase(caseData,slotType);
        currentCaseList.Add(tempCaseObject);
        tempCaseObject.SetActive(false);
    }
    
    public void MoveCase(GameObject caseObject,RectTransform targetPos)
    {
        caseObject.SetActive(true);
        RectTransform tempRect = caseObject.GetComponent<RectTransform>();
        tempRect.position = targetPos.position;
        tempRect.localScale = targetPos.localScale;
        tempRect.localRotation = Quaternion.identity;
    }

    public void MoveCase(GameObject caseObject,RectTransform targetPos,float alpha,SlotType slotType, bool isOntop = false)
    {
        MoveCase(caseObject, targetPos);
        caseObject.TryGetComponent(out CanvasGroup canvasGroup);
        if (canvasGroup)
        {
            canvasGroup.alpha = alpha;
        }else Debug.LogError("CanvasGroup is null");
        caseObject.GetComponentInChildren<PatientCase>().slotType = slotType;
        if (isOntop)
        {
            caseObject.transform.SetAsLastSibling();
        }

        if (slotType == SlotType.CURRENT_SLOT)
        {
            caseObject.GetComponentInChildren<PatientCase>().button.gameObject.SetActive(true);
        }else
        {
            caseObject.GetComponentInChildren<PatientCase>().button.gameObject.SetActive(false);
        }
        
    }
    
    public void OnPointerEnterCase(GameObject caseObject)
    {
        if (caseObject.GetComponentInChildren<PatientCase>().slotType == SlotType.LEFT_SLOT )
        {
            caseObject.GetComponent<CanvasGroup>().alpha = colorSelectedAlpha;
            RectTransform tempRect = caseObject.GetComponent<RectTransform>();
            DoTweenMoveRectTransfrom(tempRect,leftPeekSlot);
        }
        else if (caseObject.GetComponentInChildren<PatientCase>().slotType == SlotType.RIGHT_SLOT)
        {
            caseObject.GetComponent<CanvasGroup>().alpha = colorSelectedAlpha;
            RectTransform tempRect = caseObject.GetComponent<RectTransform>();
            DoTweenMoveRectTransfrom(tempRect,rightPeekSlot);
        }
        
    }
    
    public void OnpointerExitCase(GameObject caseObject)
    {
        if (caseObject.GetComponentInChildren<PatientCase>().slotType == SlotType.LEFT_SLOT)
        {
            Debug.Log("Rect moving Left Slot");
            caseObject.GetComponent<CanvasGroup>().alpha = colorAlpha;
            RectTransform tempRect = caseObject.GetComponent<RectTransform>();
            DoTweenMoveRectTransfrom(tempRect,leftslotPos);
        }else if (caseObject.GetComponentInChildren<PatientCase>().slotType == SlotType.RIGHT_SLOT)
        {
            Debug.Log("Rect moving Right Slot");
            caseObject.GetComponent<CanvasGroup>().alpha = colorAlpha;
            RectTransform tempRect = caseObject.GetComponent<RectTransform>();
            DoTweenMoveRectTransfrom(tempRect,rightslotPos);
        }
    }
    
    public void OnPointerClickCase(GameObject caseObject)
    {
        ChooseCase(caseObject);
    }

    void DoTweenMoveRectTransfrom(RectTransform currenRectTransform, RectTransform targetRectTransform)
    {
        currenRectTransform.DOMove(targetRectTransform.position,duration).SetEase(Ease.InOutSine);
        currenRectTransform.DOSizeDelta(targetRectTransform.sizeDelta,duration).SetEase(Ease.InOutSine);
        currenRectTransform.DOScale(targetRectTransform.localScale,duration).SetEase(Ease.InOutSine);
        currenRectTransform.DORotate(targetRectTransform.localEulerAngles,duration).SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// 当点击左边的Case时，左边的case移动到中间，中间的case移动到右边，右边的case隐藏
    /// 当点击右边的Case时，右边的case移动到中间，中间的case移动到左边，左边的case隐藏
    /// 当点击中间的Case时，进入关卡
    /// </summary>
    void ChooseCase(GameObject selectedCase)
    {
        PatientCase patientCase = selectedCase.GetComponentInChildren<PatientCase>();
        int listCount = currentCaseList.Count;
        
        switch(patientCase.slotType)
        {
            case SlotType.RIGHT_SLOT:
                currentIndex = (currentIndex + 1) % listCount;
                Debug.Log("Current Index: "+currentIndex);
                UpdateDisplay();
                break;
            case SlotType.LEFT_SLOT:
                currentIndex = (currentIndex - 1 + listCount) % listCount;
                Debug.Log("Current Index: "+currentIndex);
                UpdateDisplay();
                break;
            case SlotType.CURRENT_SLOT:
                Debug.Log("Enter Level");
                break;
        }
        
    }
    
    public void UpdateDisplay()
    { 
        int listCount = currentCaseList.Count;
        MoveCase(currentCaseList[currentIndex],currentSlotPos,1,SlotType.CURRENT_SLOT,true);
        int leftIndex = (currentIndex - 1 + listCount) % listCount;
        MoveCase(currentCaseList[leftIndex],leftslotPos,colorAlpha,SlotType.LEFT_SLOT);
        int rightIndex = (currentIndex + 1) % listCount;
        MoveCase(currentCaseList[rightIndex],rightslotPos,colorAlpha,SlotType.RIGHT_SLOT);
    }
}
