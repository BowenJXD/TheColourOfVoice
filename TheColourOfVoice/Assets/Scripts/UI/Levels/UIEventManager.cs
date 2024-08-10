using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] GameObject PauseUI;

    public void Resume()
    {
        Time.timeScale = 1; PauseUI.gameObject.SetActive(false);
    }
}
