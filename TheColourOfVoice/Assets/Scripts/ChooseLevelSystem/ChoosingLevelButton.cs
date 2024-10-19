using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 挂载在每关的选关按键上，用于进入关卡前的TimeLine动画播放
/// </summary>
public class ChoosingLevelButton : MonoBehaviour,IPointerEnterHandler
{
   //public int levelIndex;
   public CaseData currentCaseData;
   [SerializeField,ReadOnly] private PlayableDirector playableDirector;
   public Button button;
   [Title("关卡完成评分图标")]
   public GameObject caseSettlementIcon;
   private void Awake()
   {
      if (button!=null)
      {
         button.onClick.AddListener(OnchoosingLevelButtonClicked);
      }

      if (currentCaseData!=null)
      {
         Debug.Log("currentCaseData is" + currentCaseData.patientName);
      }else
      {
         Debug.Log("currentCaseData is null");
      }
   }

   private void OnEnable()
   {
      SceneManager.sceneLoaded += OnSceneLoaded;
   }

   private void OnDisable()
   {
      SceneManager.sceneLoaded -= OnSceneLoaded;
   }


   private void Start()
   {
      playableDirector = GameObject.Find("TimeLine").GetComponent<PlayableDirector>();
      /*if (levelIndex == 0)
      {
         Debug.LogError("LevelIndex is not set");
         return;
      }*/
      if (playableDirector == null)
      {
         Debug.LogError("PlayableDirector is not set");
         return;
      }
      
     
   }

   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
   {
      // 在这里进行场景的初始化操作
      Debug.Log("Scene " + scene.name + " loaded with mode " + mode);

      // 执行初始化逻辑
      //InitializeScene(levelIndex);
   }

   private void OnchoosingLevelButtonClicked()
   {
       PlayerPrefs.SetInt("levelIndex", currentCaseData.levelIndex);
       Lebug.Log("levelIndex", currentCaseData.levelIndex);
       Debug.Log("LevelIndex is " + currentCaseData.levelIndex);
       //TODO:这里注释的是打开timeline动画的部分
       if (currentCaseData.preLevelTimelineAsset == null)
       {
          Debug.LogError("PreLevelTimelineAsset is not set");
          return;
       }
       playableDirector.playableAsset = currentCaseData.preLevelTimelineAsset;
       GameObject.Find("ChooseLevelPanel").SetActive(false);
       Level_PsyRoom.Instance.ShowDialoguePanel(PlayTimeline);
       GameObject.Find("ChooseLevelPanel").SetActive(false);
       //levelIndex = currentCaseData.levelIndex;
       
       
       
       //ChoosingLevelData.NEXT_LEVEL_CONFIG = levelIndex;
       /*if (levelIndex == 0)
       {
          ChangeLevel(true);
          
       }else
          ChangeLevel(false);*/
   }

   private void PlayTimeline()
   {
      playableDirector.Play();
     GameObject.Find("Timeline_ani_skip_hint").SetActive(true);
   }

   /// <summary>
   /// 切换关卡
   /// </summary>
   private void ChangeLevel(bool isNextLevelTutorial = false)
   {
      if (isNextLevelTutorial)
      {
         SceneTransit.Instance.LoadTargetScene("Tutorial");
         return;
      }
      SceneTransit.Instance.LoadTargetScene("MainGame");
   }

   private void InitializeScene(int levelConfigIndex)
   {
      Debug.Log("Initializing LevelConfig");
     // GameObject.Find("LevelManager").GetComponent<LevelManager>().ChangeConfig(levelConfigIndex);
   }

   private void UpdateCaseState()
   {
      switch (currentCaseData.levelState)
      {
         
         case LevelState.Locked:
            
            break;
         case LevelState.Unlocked:
            
            break;
         case LevelState.Pass:
            InitCaseSettlementIcon("PASS");
            break;
         case LevelState.Good:
            InitCaseSettlementIcon("GOOD");
            break;
         case LevelState.Perfect:
            InitCaseSettlementIcon("PERFECT");
            break;
         default:
          break;
      }
   }
   
   /// <summary>
   /// 生成关卡结算图标
   /// </summary>
   /// <param name="caseIconName"></param>
   private void InitCaseSettlementIcon(string caseIconName)
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
   private void ResetIcon()
   {
      caseSettlementIcon.SetActive(false);
      RectTransform rect = caseSettlementIcon.GetComponent<RectTransform>();
      rect.anchoredPosition = new Vector2(609f, -172f);
      rect.sizeDelta = new Vector2(410f, 410f);
      caseSettlementIcon.GetComponent<Image>().color = new Color(1,1,1,0);
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      if (GetComponentInChildren<PatientCase>().slotType != SlotType.CURRENT_SLOT)
      {
         return;
      }
      if (GetComponentInChildren<PatientCase>().levelState == LevelState.Locked)
      {
       ChooseLevelPanel.Instance.UpdateStateText(ChooseLevelPanel.ChoosingLevelStateText.LOCKED);
      }else if (GetComponentInChildren<PatientCase>().levelState == LevelState.Unlocked)
      {
         ChooseLevelPanel.Instance. UpdateStateText(ChooseLevelPanel.ChoosingLevelStateText.ENTER_LEVEL);
      }
      else ChooseLevelPanel.Instance.UpdateStateText(ChooseLevelPanel.ChoosingLevelStateText.FINISHED);
   }
}
