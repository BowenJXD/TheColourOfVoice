using System;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteOnEnable : MonoBehaviour
{
    public static string chars = "DEFAULT TEXT";

    public SpriteRenderer sp;
    
    public static Dictionary<char, Sprite> charToSprite; 

    private void OnEnable()
    {
        if (charToSprite == null)
        {
            charToSprite = new Dictionary<char, Sprite>();
            for (int i = 33; i < 91; i++)
            {
                char c = (char) i;
                Sprite sprite = Resources.Load<Sprite>(PathDefines.CharPath + i/* + ".png"*/);
                charToSprite.Add(c, sprite);
            }
        }

        if (chars.Length == 0) return;
        char ch = chars[^1];
        if (ch != ' ')
        {
            sp.sprite = charToSprite[ch];
            chars = chars.Remove(chars.Length - 1);
        }
    }
}