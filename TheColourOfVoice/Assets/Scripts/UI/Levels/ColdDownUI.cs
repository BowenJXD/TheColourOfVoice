using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdDownUI : MonoBehaviour
{
    public Spell spell; 
    public Image mask_img;

    void Update()
    {

        if (spell != null)
        {
            float cooldown = spell.GetCooldownTime();
            float remainingCD = spell.GetRemainingCD();

            mask_img.fillAmount = remainingCD / cooldown;

        }
    }
}
