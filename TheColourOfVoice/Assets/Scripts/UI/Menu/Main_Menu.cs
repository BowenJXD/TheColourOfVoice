using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    static int index = 1;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        PlayerPrefs.SetInt("levelIndex", index);
    }
    
    public void PlayNavi()  
    {
        SceneManager.LoadSceneAsync(5);

    }

    public void NextLevel()
    {
        var idx = PlayerPrefs.GetInt("levelIndex", index);
        PlayerPrefs.SetInt("levelIndex", ++idx);
        //reload the current scene
        SceneManager.LoadSceneAsync("MainGame");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("levelIndex");
    }
}
