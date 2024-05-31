using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    int index;

    private void Awake()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);

    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(index+1);

    }

    public void Quit()
    {
        Application.Quit();
    }
}
