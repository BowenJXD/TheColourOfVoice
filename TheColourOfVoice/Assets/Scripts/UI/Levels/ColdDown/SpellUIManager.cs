using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUIManager : MonoBehaviour
{
    public GameObject spellUIPrefab;
    public Transform spellUIParent;
    public Sprite defaultSpellImage; 

    private Dictionary<Spell, GameObject> spellUIDictionary = new Dictionary<Spell, GameObject>();

    private void Start()
    {
        CreateSpellUI();
    }

    void CreateSpellUI()
    {

        foreach (var spell in SpellManager.Instance.learntSpells.Values)
        {

            if (spell.spellName == "The EveryDay Miracle")
            {
                Debug.Log("Skipping first spell: " + spell.spellName);
                continue;
            }
            
            
            GameObject spellUI = Instantiate(spellUIPrefab, spellUIParent);
            spellUI.name = spell.spellName;
            spellUIDictionary.Add(spell, spellUI);
            
            Image spellIcon = spellUI.transform.Find("SpellIcon")?.GetComponent<Image>();
            if (spell.spellImage != null)
            {
                spellIcon.sprite = spell.spellImage;  
            }
            else
            {
                Debug.LogWarning($"Spell {spell.spellName} has no image. Using default image.");
                spellIcon.sprite = defaultSpellImage; 
            }
            /*
            spellIcon.sprite = spell.spellImage;
            */

            Image cooldownMask = spellUI.transform.Find("CooldownMask")?.GetComponent<Image>();
            cooldownMask.sprite = spellIcon.sprite;
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
                cooldownText.text = Mathf.CeilToInt(remainingTime).ToString(); 
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