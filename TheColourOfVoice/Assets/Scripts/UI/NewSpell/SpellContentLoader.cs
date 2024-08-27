using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellContentLoader : MonoBehaviour
{
    public int spellIndex;
        
    public TMP_Text spellName;
    public TMP_Text spellDescription;
    public TMP_Text spellCooldown;
    public TMP_Text placeholder;
    public SpriteRenderer spellImage;
    public TrailRenderer trailRenderer;
    public ParticleSystem particleSystem;
    public Image bgColour;
    public float bgColourIntensity = 4;
    public TMP_InputField spellInputField;
    public SaveData saveData;
    
    private void Start()
    {
        spellIndex = PlayerPrefs.GetInt("levelIndex", spellIndex);
        var data = ResourceManager.Instance.LoadCSV(PathDefines.SpellConfig);
        var spellData = data[spellIndex.ToString()];
        spellName.text = spellData["Name"];
        spellDescription.text = spellData["Description"];
        spellCooldown.text = spellData["Cooldown"];
            
        string themeColour = spellData["ThemeColour"];
        PaintColor color = (PaintColor)Enum.Parse(typeof(PaintColor), themeColour);
        ColorManager.Instance.gradientDict.TryGetValue(color, out var paintColor);
        trailRenderer.colorGradient = paintColor;
        var main = particleSystem.main;
        main.startColor = paintColor;
        bgColour.color = ColorManager.Instance.GetColor(color) / bgColourIntensity;
        bgColour.color = new Color(bgColour.color.r, bgColour.color.g, bgColour.color.b, 1);
            
        string logoPath = PathDefines.SpellLogoPath + spellData["LogoPath"];
        spellImage.sprite = Resources.Load<Sprite>(logoPath);
        
        string defaultTriggerWord = spellData["Default"];
        if (placeholder) placeholder.text = defaultTriggerWord;
        
        if (saveData)
        {
                
            spellInputField.onSubmit.AddListener(OnSubmit);
        }
    }
    
    void OnSubmit(string input)
    {
        while (saveData.spellTriggerWords.Count <= spellIndex-1)
        {
            saveData.spellTriggerWords.Add("");
        }
        saveData.spellTriggerWords[spellIndex-1] = input;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("levelIndex");
    }
}