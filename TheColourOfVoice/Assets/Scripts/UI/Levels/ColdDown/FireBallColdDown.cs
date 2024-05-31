using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallColdDown : MonoBehaviour
{

    public GameObject parent; 
    private Spell spell; 
    private Image mask_img;
    public Text cd_text;

    private void Start()
    {

        Transform ImagechildTransform = parent.transform.Find("ImageFilled");
        Transform TextchildTransform = parent.transform.Find("CD");

        mask_img = ImagechildTransform.GetComponent<Image>();
        cd_text = TextchildTransform.GetComponent<Text>();
        if (mask_img != null)
        {
            spell = FindObjectOfType<FireBallSpell>();
            mask_img.sprite = spell.spellImage;
        }
        
    }

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
