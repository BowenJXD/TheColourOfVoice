using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerManger : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public string nextSceneName; 

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
