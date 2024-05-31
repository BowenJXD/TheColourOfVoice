using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdDownUI : MonoBehaviour
{
    public Spell spell; 
    public Image mask_img;
    public Text cd_text;
    
    void Update()
    {

        if (spell != null)
        {
            float cooldown = spell.GetCooldownTime();
            float remainingCD = spell.GetRemainingCD();
            cd_text.text = remainingCD.ToString("F1");
            
            if (remainingCD > 0)
            {
                cd_text.gameObject.SetActive(true);
                cd_text.text = remainingCD.ToString("F1");
                mask_img.fillAmount = remainingCD / cooldown;
            }
            else
            {
                cd_text.gameObject.SetActive(false);
                mask_img.fillAmount = 0;
            }
        }else
        {
            cd_text.gameObject.SetActive(false);
            mask_img.fillAmount = 0;
        }
    }
}
