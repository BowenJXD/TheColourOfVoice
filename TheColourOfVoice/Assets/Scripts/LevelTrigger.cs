using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LevelTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public PlayableAsset playableAsset;
    [SerializeField] private bool isInDialogueArea = false;
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isInDialogueArea) 
        {
            playTimeline();
        }
    }
    public void playTimeline() 
    {
        if (playableAsset == null) return;
        if (playableDirector == null) return;

        playableDirector.playableAsset = this.playableAsset;
        playableDirector.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            isInDialogueArea = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInDialogueArea = false;
        }
    }
}
