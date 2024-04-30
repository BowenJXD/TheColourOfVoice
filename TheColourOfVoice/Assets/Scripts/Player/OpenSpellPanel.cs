using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSpellPanel : MonoBehaviour
{
    public SpellPanel spellPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!spellPanel.gameObject.activeSelf)
            {
                //Debug.Log("OpenspellPanel");
                Time.timeScale = 0;
                //spellPanel.gameObject.SetActive(true);
                spellPanel.OpenSpellPanel();
            }
            else { Time.timeScale = 1; spellPanel.gameObject.SetActive(false); }
            
        }

     
    }

   
}
