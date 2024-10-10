using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information about the player.
/// </summary>
[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData", order = 1)]
public class SaveData : ScriptableObject
{
    public List<int> levelStars = new List<int>();
    
    public List<string> spellTriggerWords = new List<string>();
}