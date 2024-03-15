using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads a file that converts characters to tile patterns.
/// TODO: Add support for different pattern width. 
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
    
    public static readonly int patternWidth = 5;
    public static readonly int patternHeight = 5;
    
    public TextAsset csvFile;

    protected override void Awake()
    {
        base.Awake();
        if (csvFile != null)
        {
            LoadConfig(csvFile.text);
        }
    }

    int patternStartWidth => - patternWidth / 2;
    int patternStartHeight => patternHeight / 2;

    void LoadConfig(string csv)
    {
        charPatterns = new Dictionary<char, List<Vector2Int>>();
        string[] lines = csv.Split('\n');
        char currentChar = ' ';
        List<Vector2Int> pattern = new List<Vector2Int>();
        int y = patternStartHeight;
        
        foreach (var line in lines)
        {
            if (line == "") continue;
            // character indicator sign
            if (!"01,".Contains(line[0]))
            {
                if (pattern.Count > 0)
                {
                    charPatterns.Add(currentChar, pattern);
                    pattern = new List<Vector2Int>();
                }
                currentChar = line[0];
                y = patternStartHeight;
            }
            else
            {
                // pattern content (trim the last char: "/r")
                string[] values = line[..^1].Split(',');
                for (int x = 0; x < values.Length; x++)
                {
                    // if the cell is empty, put false, else true
                    if (values[x] != "")
                    {
                        pattern.Add(new Vector2Int(x + patternStartWidth, y));
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
}