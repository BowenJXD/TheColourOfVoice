using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    private PlayableDirector playableDirector;
    //public ArticyReference articyReference;
    public enum CharacterName
    {
        Ava,
        LittleWitch,
        Narrator,
        Ron,
        Unknown,
        Hilda,
        Nioneisos,
        Chloe,
        Benjamin
    }
    public CharacterName characterName;
    [TextArea(8,1)] public string dialogueLine;
    public int dialogueSize = 60;

    private bool isClipPlayed;
    public bool requirePause;
    private bool pauseScheduled;

    public override void OnPlayableCreate(Playable playable)
    {
        playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
    }
    
    /// <summary>
    /// TimeLine的Update方法
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    /// <param name="playerData"></param>
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (isClipPlayed == false && info.weight >0)
        {
            //TODO：这里需要进行对话内容的赋值
            Level_PsyRoom.Instance.showDialogue(dialogueLine, GetCharacterName(), dialogueSize);
            if (requirePause)
            {
                pauseScheduled = true;
                //isClipPlayed = true;
            }
            isClipPlayed = true;
        }
    }
    
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        isClipPlayed = false;
        
        if (pauseScheduled)
        {
            //暂停TimeLine的播放
            Debug.Log("Clip is stopped!");
            pauseScheduled = false;
            Level_PsyRoom.Instance.PauseTimeline(playableDirector);
        }
        else
        {
            Debug.Log("Clip not stopped");
            //Level_PsyRoom.Instance.ToggleDialogueBox(false);
        }
    }
  
    //将枚举类型转换为字符串
    public string GetCharacterName()
    {
        return characterName.ToString();
    }
}
