using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColourBlindMenu : MonoBehaviour
{
    public string SceneName;
    public GameObject GlobalVolume;
    private Slider[] Sliders;

    private void Start()
    {
        Sliders = GetComponentsInChildren<Slider>();
    }

    public void MenuButton()
    {
        Destroy(GlobalVolume);
        SceneManager.LoadScene(SceneName);
    }

    public void ResetButton()
    {
        for (int i = 0; i < Sliders.Length; i++)
        {
            Sliders[i].value = (float)i / Sliders.Length;
        }
    }
}