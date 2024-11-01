using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    static int index = 1;
    public static bool isInputDisabled = false;  // 静态标志位，决定是否允许输入

    // Update is called once per frame
    void Update()
    {
        // 仅在输入没有被禁用时才允许处理输入
        if (!isInputDisabled && Input.anyKeyDown && SceneManager.GetActiveScene().name == "MainMenu")
        {
            // 切换到游戏场景
            PlayGame();
        }
    }
    public void PlayGame()
    {
        //SceneTransit.Instance.LoadTargetScene("ColourBlindSetting");
        SceneManager.LoadScene("Opening");
        index = SaveDataManager.Instance.saveData.levelsCompleted;
        PlayerPrefs.SetInt("levelIndex", index);
        Lebug.Log("levelIndex", index);
    }
    
    public void PlayNavi()  
    {
        SceneManager.LoadSceneAsync("Opening");

    }

    public void NextLevel()
    {
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
