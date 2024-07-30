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
        //this.spellInputField.text = spellName;
    }
    public Sprite spellImage;
    public float coolDown;
    public string Intro;
    public string spellName;
    //public InputField spellInputField;
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
   

    
    private void Awake()
    {
        if (SpellManager.Instance == null) return;

        var currentSpellList = SpellManager.Instance.learntSpells;
        foreach (var kvp in currentSpellList) 
        {
            spellNameList.Add(kvp.Key);
            var spell = kvp.Value;
            SubSpellPanelData tempSpellData = new SubSpellPanelData(spell.spellImage,spell.cooldown, spell.spellDescription, spell.triggerWords);
            CreateSubSpellPanelData(spell);
            spellDataList.Add(tempSpellData);

        }

    }

    private void CreateSubSpellPanelData(Spell spell) 
    {
        SubSpellPanel sub =Instantiate(Resources.Load<SubSpellPanel>(PathDefines.SubSpellPanelPath));
        sub.transform.SetParent(transform);
        sub.transform.localScale = Vector3.one;
        RectTransform reactransform = sub.GetComponent<RectTransform>();
        reactransform.anchoredPosition = new Vector3(54.53935f, -110.4518f, 0);
        subSpellPanelList.Add(sub.GetComponent<SubSpellPanel>());
        sub.spell = spell;
        sub.InitSubSpellPanel();
        
    }

    /// <summary>
    /// 打开SubPanel的第一面
    /// </summary>
    public void OpenSpellPanel() 
    {
        this.gameObject.SetActive(true);
       // subSpellPanelList[0].gameObject.SetActive(true);
        subSpellPanelList[0].OpenSubSpellPanel();
    }

    public void NextPage()
    {
        foreach (var spellPanel in subSpellPanelList)
        {
            if (spellPanel.gameObject.activeSelf)
            {
                spellPanel.gameObject.SetActive(false);
                int index = subSpellPanelList.IndexOf(spellPanel);
                if (index == subSpellPanelList.Count - 1)
                {
                    subSpellPanelList[0].OpenSubSpellPanel();
                }
                else
                {
                    subSpellPanelList[index + 1].OpenSubSpellPanel();
                }
                break;
            }
        }
    }
    
    public void LastPage()
    {
        foreach (var spellPanel in subSpellPanelList)
        {
            if (spellPanel.gameObject.activeSelf)
            {
                spellPanel.gameObject.SetActive(false);
                int index = subSpellPanelList.IndexOf(spellPanel);
                if (index == 0)
                {
                    subSpellPanelList[subSpellPanelList.Count - 1].OpenSubSpellPanel();
                }
                else
                {
                    subSpellPanelList[index - 1].OpenSubSpellPanel();
                }
                break;
            }
        }
    }
}
