using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads a file that converts characters to tile patterns.
/// The index of patterns are from the left center of a character.
/// <example><br></br>
/// a <br></br>
/// 0 0 1 0 0 <br></br>
/// 0 1 0 1 0 <br></br>
/// 0 1 1 1 0 <br></br>
/// 0 1 0 1 0 <br></br>
/// 0 1 0 1 0 </example>
/// </summary>
public class TextToTile : Singleton<TextToTile>
{
    private Dictionary<char, List<Vector2Int>> charPatterns;
    
    private Dictionary<char, int> charWidths;

    public static readonly int patternHeight = 5;
    
    public TextAsset csvFile;
    
    public int spaceWidth = 2;

    protected override void Awake()
    {
        base.Awake();
        if (csvFile != null)
        {
            LoadConfig(csvFile.text);
        }
    }

    int patternStartHeight => patternHeight / 2;

    void LoadConfig(string csv)
    {
        charPatterns = new ();
        charPatterns.Add(' ', new ());
        charWidths = new ();
        charWidths.Add(' ', spaceWidth);
        string[] lines = csv.Split('\n');
        char[] currentChar = Array.Empty<char>();
        List<Vector2Int> pattern = new List<Vector2Int>();
        int y = -patternStartHeight - 1;
        int patternWidth = 0;
        
        foreach (var line in lines)
        {
            if (line == "") continue;
            
            // pattern content (trim the last char: "/r")
            string[] values = line[..^1].Split(',');
            
            // character indicator sign
            if (y < -patternStartHeight)
            {
                if (pattern.Count > 0)
                {
                    foreach (char c in currentChar)
                    {
                        if (charPatterns.ContainsKey(c))
                        {
                            continue;
                        }
                        charPatterns.Add(c, pattern);
                        charWidths.Add(c, patternWidth);
                    }
                    pattern = new List<Vector2Int>();
                }
                currentChar = values[0].ToCharArray();
                // width is the second char
                patternWidth = int.Parse(values[1]);
                y = patternStartHeight;
            }
            else
            {
                for (int x = 0; x < patternWidth; x++)
                {
                    // if the cell is empty, put false, else true
                    if (values[x] != "")
                    {
                        pattern.Add(new Vector2Int(x, y));
                    }
                }
            
                y--;
            }
        }
    }

    public List<Vector2Int> GetPattern(char character)
    {
        return charPatterns[character];
    }
    
    public int GetPatternWidth(char character)
    {
        return charWidths[character];
    }
}