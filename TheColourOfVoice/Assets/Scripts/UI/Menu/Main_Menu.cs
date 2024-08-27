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
        SceneTransit.Instance.LoadTargetScene("MainGame");
        PlayerPrefs.SetInt("levelIndex", index);
    }
    
    public void PlayNavi()  
    {
        SceneManager.LoadSceneAsync("PlayerNavi");

    }

    public void NextLevel()
    {
        var idx = PlayerPrefs.GetInt("levelIndex", index);
        PlayerPrefs.SetInt("levelIndex", ++idx);
        SceneManager.LoadSceneAsync("NewSpell");
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
