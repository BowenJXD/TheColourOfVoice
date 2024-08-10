using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] GameObject PauseUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*if (Time.timeScale == 0 && !PauseUI.gameObject.activeSelf) return;*/ // prevent opening the spell panel when the game is paused
            if (!PauseUI.gameObject.activeSelf)
            {
                //Debug.Log("PausePanel");
                Time.timeScale = 0;
                PauseUI.gameObject.SetActive(true);

            }
            else { Time.timeScale = 1; PauseUI.gameObject.SetActive(false); }
            
        }
    }
}
