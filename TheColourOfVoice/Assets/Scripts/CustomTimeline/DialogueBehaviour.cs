using System.Collections;
using System.Collections.Generic;
using Articy.Unity;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    private PlayableDirector playableDirector;
    //public ArticyReference articyReference;
    public string characterName;
    [TextArea(8,1)] public string dialogueLine;
    public int dialogueSize;

    private bool isClipPlayed;
    public bool requirePause;
    private bool pauseScheduled;

    public override void OnPlayableCreate(Playable playable)
    {
        playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
    }
    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (isClipPlayed == false && info.weight >0)
        {
            //TODO：这里需要进行对话内容的赋值

            if (requirePause)
            {
                pauseScheduled = true;
                isClipPlayed = true;
            }
        }
    }
    
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        isClipPlayed = false;
        Debug.Log("Clip is stopped!");
        if (pauseScheduled)
        {
            //暂停TimeLine的播放
            //playableDirector.Pause();
            pauseScheduled = false;
            Level_PsyRoom.Instance.PauseTimeline(playableDirector);
        }
        else
        {
            //TODO 将对话框进行关闭
        }
    }
  
}
