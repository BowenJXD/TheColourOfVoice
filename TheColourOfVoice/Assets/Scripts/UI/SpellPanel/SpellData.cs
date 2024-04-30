using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Data/SpellData")]
public class SpellData : ScriptableObject
{
    public string speaker;
    [Multiline] public string introContent;
    
}
