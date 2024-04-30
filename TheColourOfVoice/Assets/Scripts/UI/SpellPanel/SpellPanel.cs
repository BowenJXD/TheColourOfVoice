using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpellPanel : MonoBehaviour
{
    //private List<>
    public TextMeshProUGUI spellNameUGUI;
    [SerializeField]
    private List<String> SpellNameList;

    private void Start()
    {
        if (SpellManager.Instance != null)
        {
            List<Spell> currentSpellList = SpellManager.Instance.GetComponentsInChildren<Spell>().ToList();
            foreach (Spell spell in currentSpellList) 
            {
                SpellNameList.Add(spell.spellName);
            }
        }
    }

    private void InitSubSpellPanel() 
    {

    }
}
