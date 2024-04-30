using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubSpellPanel : MonoBehaviour
{
    public Spell spell;
    public Image spellImage;
    public TextMeshProUGUI spellIntro;
    public TextMeshProUGUI spellName;
    public float coolDown;

    public void InitSubSpellPanel() 
    {
        spellImage.sprite = spell.spellImage;
        spellIntro.text = spell.spellDescription;
        spellName.text = spell.spellName;
        coolDown = spell.cooldown;
    }

    public void UpdateSpellName(string spellName) 
    {

    }
}
