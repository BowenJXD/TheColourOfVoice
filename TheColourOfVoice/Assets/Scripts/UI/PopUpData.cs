using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BubbleData_", menuName = "Data/BubbleData")]
public class PopUpData : ScriptableObject
{
    public enum CharacterName
    {
        Ava_CASE1,
        Benjamin_CASE2,
        Chloe_CASE3,
        Ron_CASE5,
        Nioneisos_CASE6,
        LittleWitch,
        Unknown,
    }
    [Header("角色名字")]
    public CharacterName characterName;
    [Header("弹窗内容")]
    [Multiline]public string bubbleText;
    public float timeToDisplay = 5f;
    public UnityEvent onFinishedEvent;
}
