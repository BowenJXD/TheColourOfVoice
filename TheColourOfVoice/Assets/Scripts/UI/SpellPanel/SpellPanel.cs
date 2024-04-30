using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SubSpellPanelData 
{
   public SubSpellPanelData(Sprite spellImage,float coolDown, string intro, string spellName)
    {
        this.spellImage = spellImage;
        this.coolDown = coolDown;
        this.Intro = intro;
        this.spellName = spellName;
    }
    public Sprite spellImage;
    public float coolDown;
    public string Intro;
    public string spellName;
}

public class SpellPanel : MonoBehaviour
{
    [SerializeField]
    private List<SubSpellPanel> subSpellPanelList;
    public TextMeshProUGUI spellNameUGUI;
    [SerializeField]
    private List<String> spellNameList;
    [SerializeField]
    private List<SubSpellPanelData> spellDataList;
    private void Start()
    {
        if (SpellManager.Instance == null) return;
        
        List<Spell> currentSpellList = SpellManager.Instance.GetComponentsInChildren<Spell>().ToList();
        foreach (Spell spell in currentSpellList) 
        {
            spellNameList.Add(spell.spellName);
            SubSpellPanelData tempSpellData = new SubSpellPanelData(spell.spellImage,spell.cooldown, spell.spellDescription, spell.spellName);
            CreateSubSpellPanelData(spell);
            spellDataList.Add(tempSpellData);

        }

    }

    private void CreateSubSpellPanelData(Spell spell) 
    {
        SubSpellPanel sub =Instantiate(Resources.Load<SubSpellPanel>("Prefabs/SubSpellPanel"));
        sub.transform.SetParent(transform);
        sub.transform.localScale = Vector3.one;
        RectTransform reactransform = sub.GetComponent<RectTransform>();
        reactransform.anchoredPosition = new Vector3(54.53935f, -110.4518f, 0);
        subSpellPanelList.Add(sub.GetComponent<SubSpellPanel>());
        sub.spell = spell;
        sub.InitSubSpellPanel();
        //sub.gameObject.SetActive(false);
        
    }

  
}
