using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUIManager : Singleton<SpellUIManager>
{
    public GameObject spellUIPrefab;
    public Transform spellUIParent;
    public Sprite defaultSpellImage; 
    
    [Title("Magic Circle")]
    public Image magicCircleInnerCircle;
    public Image magicCircleOMiddleCircle;
    public float magicCircleInnerCircleSpeed = 1f;
    public float magicCircleMiddleCircleSpeed = 1f;

    //技能冷却结束的时候显示的特效
    [SerializeField,ReadOnly] private GameObject spellCdCompleteFlashPrefab;

     private Dictionary<Spell, GameObject> spellUIDictionary = new Dictionary<Spell, GameObject>();

    private void Start()
    {
        CreateSpellUI();
    }
    
    /// <summary>
    /// 当技能冷却结束时，显示技能冷却结束的特效
    /// </summary>
    /// <param name="spell">技能</param>
    public void OnSkillCDComplete(Spell spell)
    {
        if (spellUIDictionary.TryGetValue(spell, out var spellUI))
        {
            GameObject spellCdCompleteFlash = spellUI.transform.Find("CoolDownFlash(Clone)").gameObject;
            spellCdCompleteFlash.SetActive(true);
        }
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
            
            //初始化技能UI
            GameObject spellUI = Instantiate(spellUIPrefab, spellUIParent);
            spellUI.TryGetComponent<RectTransform>(out RectTransform rect);
            switch (spell.spellIndex)
            {
                case 1:
                    rect.localPosition = spellUIParent.transform.Find("SkillSlot_1").localPosition;
                    break;
                case 2:
                    rect.localPosition = spellUIParent.transform.Find("SkillSlot_2").localPosition;
                    break;
                case 3:
                    rect.localPosition = spellUIParent.transform.Find("SkillSlot_3").localPosition;
                    break;
                case 4:
                    rect.localPosition = spellUIParent.transform.Find("SkillSlot_4").localPosition;
                    break;
                case 5:
                    rect.localPosition = spellUIParent.transform.Find("SkillSlot_5").localPosition;
                    break;
            }
            
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
            
            //初始化技能冷却结束的特效，添加作为子物体
            spellCdCompleteFlashPrefab = Resources.Load<GameObject>("Prefabs/CoolDownFlash");
            if (spellCdCompleteFlashPrefab != null)
            {
                GameObject spellCdCompleteFlash = Instantiate(spellCdCompleteFlashPrefab, spellUI.transform, false);
                spellCdCompleteFlash.transform.localPosition = new Vector3(-7f,-6f,0f);
                spellCdCompleteFlash.transform.localRotation = Quaternion.identity;
                spellCdCompleteFlash.transform.localScale = Vector3.one;
                
                //spellCdCompleteFlash.SetActive(false);
                var mainModule = spellCdCompleteFlash.GetComponent<ParticleSystem>().main;
                mainModule.stopAction = ParticleSystemStopAction.Disable;
            }
            
        }
    }

    private void Update()
    {
        //RotateMagicCircle
        RotateMagicCircle(magicCircleInnerCircleSpeed, magicCircleInnerCircle, true);
        RotateMagicCircle(magicCircleMiddleCircleSpeed, magicCircleOMiddleCircle, false);
        
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
    
    private void RotateMagicCircle(float speed, Image image, bool rotateClockwise)
    {
        float direction = rotateClockwise ? -1 : 1;
        image.rectTransform.Rotate(Vector3.forward, speed * direction * Time.deltaTime);
    }
    
   
}