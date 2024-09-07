using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BubbleData_", menuName = "Data/BubbleData")]
public class PopUpData : ScriptableObject
{
    public enum CharacterName
    {
        Ava,
        LittleWitch,
        Narrator,
        Ron,
        Unknown,
        Hilda,
        Nioneisos,
        Chloe,
        Benjamin
    }
    [Header("角色名字")]
    public CharacterName characterName;
    [Header("弹窗内容")]
    [Multiline]public string bubbleText;
    public float timeToDisplay = 5f;
    public UnityEvent onFinishedEvent;
}
