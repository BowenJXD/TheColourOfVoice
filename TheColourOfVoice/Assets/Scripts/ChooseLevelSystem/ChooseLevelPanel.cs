using System;
using System.Collections;
using System.Collections.Generic;
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
    public float colorAlpha = 83f;
    
    //自身板块
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
        /*InstantiateCase(currentSlotPos,caseDataList[currentIndex],SlotType.CURRENT_SLOT);
        InstantiateCase(leftslotPos,caseDataList[(currentIndex+1) % caseDataList.Count],SlotType.LEFT_SLOT);
        InstantiateCase(rightslotPos,caseDataList[(currentIndex+2) % caseDataList.Count],SlotType.RIGHT_SLOT);*/
        foreach (var caseData in caseDataList)
        {
            Debug.Log("CaseData: "+caseData.patientName);
            InstantiateCase(currentSlotPos,caseData,SlotType.CURRENT_SLOT);
        }
    }
    
    void InstantiateCase(RectTransform rectTransform,CaseData caseData,SlotType slotType)
    {
        GameObject tempCaseObject = Instantiate(Resources.Load<GameObject>("Prefabs/CaseTest"),transform);
        RectTransform tempCaseRect = tempCaseObject.GetComponent<RectTransform>();
        tempCaseObject.transform.position = rectTransform.position;
        tempCaseObject.GetComponent<PatientCase>().InstantiateCase(caseData,slotType);
        currentCaseList.Add(tempCaseObject);
    }
    
    
   
}
