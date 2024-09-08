using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUIManager : MonoBehaviour
{
    public GameObject spellUIPrefab;
    public Transform spellUIParent;

    private Dictionary<Spell, GameObject> spellUIDictionary = new Dictionary<Spell, GameObject>();

    private void Start()
    {
        CreateSpellUI();
    }

    void CreateSpellUI()
    {
        int spellIndex = 0; 

        foreach (var spell in SpellManager.Instance.learntSpells.Values)
        {

            if (spellIndex == 0)
            {
                spellIndex++;
                Debug.Log("Skipping first spell: " + spell.spellName);
                continue;
            }
            
            if (spell == null)
            {
                Debug.LogError("Spell is null in learntSpells!");
                spellIndex++;
                continue;
            }
            
            GameObject spellUI = Instantiate(spellUIPrefab, spellUIParent);
            spellUIDictionary.Add(spell, spellUI);
            
            Image spellIcon = spellUI.transform.Find("SpellIcon")?.GetComponent<Image>();
            spellIcon.sprite = spell.spellImage;

            Image cooldownMask = spellUI.transform.Find("CooldownMask")?.GetComponent<Image>();
            cooldownMask.fillAmount = 0;

            TextMeshProUGUI cooldownText = spellUI.transform.Find("CooldownText")?.GetComponent<TextMeshProUGUI>();
            cooldownText.enabled = false;
        }
    }

    private void Update()
    {
        foreach (var kvp in spellUIDictionary)
        {
            Spell spell = kvp.Key;
            GameObject spellUI = kvp.Value;

            Image cooldownMask = spellUI.transform.Find("CooldownMask").GetComponent<Image>();
            TextMeshProUGUI cooldownText = spellUI.transform.Find("CooldownText").GetComponent<TextMeshProUGUI>();

            if (spell.isInCD)
            {
                float cooldownProgress = spell.GetRemainingCD() / spell.GetCooldownTime();
                cooldownMask.fillAmount = cooldownProgress; 

                float remainingTime = spell.GetRemainingCD();
                cooldownText.text = Mathf.CeilToInt(remainingTime).ToString(); // 显示整数秒数
                cooldownText.enabled = true;
            }
            else
            {
                cooldownMask.fillAmount = 0;
                cooldownText.enabled = false;
            }
        }
    }
}