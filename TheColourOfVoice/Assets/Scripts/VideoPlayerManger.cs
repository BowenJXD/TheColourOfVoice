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
    
    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
