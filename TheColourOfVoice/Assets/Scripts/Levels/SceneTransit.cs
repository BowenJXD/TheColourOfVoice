using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransit : Singleton<SceneTransit>
{
    public string loadingSceneName = "Loading";
    public float minimumLoadTime = 3.0f;
    public Slider progressBar;
    
    public void LoadTargetScene(string targetSceneName)
    {
        StartCoroutine(LoadScenes(targetSceneName));
        DontDestroyOnLoad(this);
    }

    private IEnumerator LoadScenes(string targetSceneName)
    {
        // Start loading the loading screen scene (Scene C) but don't activate it immediately
        AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);
        while (!loadLoadingScene.isDone)
        {
            yield return null;
        }

        // Set the loading screen as the active scene to display it
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingSceneName));

        // Start loading the target scene (Scene B) but don't activate it immediately
        AsyncOperation loadTargetScene = SceneManager.LoadSceneAsync(targetSceneName);
        loadTargetScene.allowSceneActivation = false;

        float elapsedTime = 0f;

        // Wait until the target scene is 90% loaded and keep track of the elapsed time
        while (loadTargetScene.progress < 0.9f || elapsedTime < minimumLoadTime)
        {
            if (progressBar) progressBar.value = Mathf.Clamp01(loadTargetScene.progress / 0.9f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Activate the target scene (Scene B) after the minimum load time has passed
        loadTargetScene.allowSceneActivation = true;

        // Wait until the target scene is fully loaded
        while (!loadTargetScene.isDone)
        {
            yield return null;
        }
    }
}