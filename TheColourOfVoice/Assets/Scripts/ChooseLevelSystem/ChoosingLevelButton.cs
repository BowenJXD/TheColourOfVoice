using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 挂载在每关的选关按键上，用于进入关卡前的TimeLine动画播放
/// </summary>
public class ChoosingLevelButton : MonoBehaviour
{
   public int levelIndex;
   public CaseData currentCaseData;
   [SerializeField,ReadOnly] private PlayableDirector playableDirector;
   public Button button;
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
      InitializeScene(levelIndex);
   }

   private void OnchoosingLevelButtonClicked()
   {
      /*
       //TODO:这里注释的是打开timeline动画的部分
       if (currentCaseData.preLevelTimelineAsset == null)
      {
         Debug.LogError("PreLevelTimelineAsset is not set");
         return;
      }
      playableDirector.playableAsset = currentCaseData.preLevelTimelineAsset;
      GameObject.Find("ChooseLevelPanel").SetActive(false);
      Level_PsyRoom.Instance.ShowDialoguePanel(PlayTimeline);*/
      GameObject.Find("ChooseLevelPanel").SetActive(false);
      levelIndex = currentCaseData.levelIndex;
      if (levelIndex == 0)
      {
         ChangeLevel(true);
         
      }else
         ChangeLevel(false);
   }

   private void PlayTimeline()
   {
      playableDirector.Play();
   }

   /// <summary>
   /// 切换关卡
   /// </summary>
   private void ChangeLevel(bool isNextLevelTutorial = false)
   {
      if (isNextLevelTutorial)
      {
         SceneManager.LoadScene("Tutorial");
         return;
      }
      SceneManager.LoadScene("MainGame");
   }

   private void InitializeScene(int levelConfigIndex)
   {
      GameObject.Find("LevelManager").GetComponent<LevelManager>().ChangeConfig(levelConfigIndex);
   }
}
