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
    public TMP_InputField spellInputField;
    public float coolDown;
    
    private string currentSpellName;
    public void InitSubSpellPanel() 
    {
        spellImage.sprite = spell.spellImage;
        spellIntro.text = spell.spellShortDescription;
        spellName.text = spell.spellName;
        coolDown = spell.cooldown;
        spellInputField.text = spell.spellName;
        spellInputField.onSelect.AddListener(delegate { OnInputFieldStartEdit(); });
        spellInputField.onEndEdit.AddListener(delegate { OnInputFieldEndEdit(); });
        gameObject.SetActive(false);
    }

    public void OpenSubSpellPanel() 
    {
        gameObject.SetActive(true);
        //隐藏InputField
        spellInputField.gameObject.SetActive(false);
    }
    
    public void CloseSubSpellPanel() 
    {
        gameObject.SetActive(false);
    }
    public void OpenInputField() 
    {
        spellInputField.gameObject.SetActive(true);
        spellInputField.Select();
        spellInputField.ActivateInputField();
    }
    
    public void OnInputFieldStartEdit() 
    {
       //spellInputField.gameObject.SetActive(true);
       currentSpellName = spell.spellName;
        Debug.Log("Edit Start, current spellName: " + spell.spellName);
    }
    
    /// <summary>
    /// 改变咒语内容
    /// </summary>
    public void OnInputFieldEndEdit() 
    {
        string tempSpellName = spellInputField.text;
        spell.spellName = tempSpellName;
        SpellManager.Instance.ChangeName(currentSpellName,tempSpellName);
        spellName.text = tempSpellName;
        spellInputField.gameObject.SetActive(false);
        Debug.Log("Edit End, new spellName: " + spell.spellName);
    }
  
}
