using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

/// <summary>
/// 挂载在每关的选关按键上，用于进入关卡前的TimeLine动画播放
/// </summary>
public class ChoosingLevelButton : MonoBehaviour
{
   public PlayableDirector playableDirector;
   private Button button;
   private void Awake()
   {
      button = GetComponent<Button>();
      button.onClick.AddListener(PlayTimeline);
   }
   
   private void PlayTimeline()
   {
      playableDirector.Play();
   }
}
