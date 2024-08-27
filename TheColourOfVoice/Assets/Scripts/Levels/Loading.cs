using System;
using TMPro;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public float cd = 1;
    
    public TMP_Text text;
    
    string[] texts = new string[]
    {
        "If the voice input does not work, give it another shot with a louder voice.",
        "Eliminating enemies won't give any score, it only prevents them from erasing the painting.",
        "The more you paint, the faster the score will grow.",
        "Press TAB to open the spell menu, where you can see your spells and change their names.",
        "A quiet environment is recommended for a more accurate voice input.",
        "Spells would have different effects depending on the trigger word and how you say it.",
    };

    private int currentTextIndex = 0;

    private void Start()
    {
        ChangeText();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !IsInvoking(nameof(ChangeText)))
        {
            Invoke(nameof(ChangeText), cd);
        }
    }
    
    void ChangeText()
    {
        int randomIndex = UnityEngine.Random.Range(0, texts.Length);
        if (randomIndex == currentTextIndex)
        {
            randomIndex = (randomIndex + 1) % texts.Length;
        }
        currentTextIndex = randomIndex;
        text.text = texts[currentTextIndex];
    }
}