using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BigMap : Singleton<BigMap>
{
    [Header("Benj")]
    public PlayableAsset benj_timeline;
    
    [Header("Dialogue for Benj")]
    public SequenceEventExecutor benj_preSequence;

    public PlayableDirector playableDirector;
    
    protected override void Awake()
    {
        base.Awake();


    }
    public void StartDialogueSequence(SequenceEventExecutor sequenceEventExecutor) 
    {
        Debug.Log("Start Benji Dialogue");
        if (sequenceEventExecutor == null) 
        {
            Debug.Log("Failure to excute Benji Dialogue");
            return;
        }

        sequenceEventExecutor.Init(OnFinishedEvent);
        sequenceEventExecutor.Excute();
    }

    public void playTimeline(Trigger_Type trigger_Type)
    {
        if (playableDirector == null) return;
        switch (trigger_Type) 
        {
            case Trigger_Type.Trigger_Benj:
                playableDirector.playableAsset = benj_timeline;
                break;

                default: break;
        }
        
        playableDirector.Play();
    }



    void OnFinishedEvent(bool success)
    {
        Debug.Log(success);
    }
}
