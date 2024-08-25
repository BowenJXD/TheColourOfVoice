using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
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

   private void OnchoosingLevelButtonClicked()
   {
      if (currentCaseData.preLevelTimelineAsset == null)
      {
         Debug.LogError("PreLevelTimelineAsset is not set");
         return;
      }
      playableDirector.playableAsset = currentCaseData.preLevelTimelineAsset;
      GameObject.Find("ChooseLevelPanel").SetActive(false);
      Level_PsyRoom.Instance.ShowDialoguePanel(PlayTimeline);
      
   }

   private void PlayTimeline()
   {
      playableDirector.Play();
   }
   
   
}
