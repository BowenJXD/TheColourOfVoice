
using UnityEngine;
using UnityEngine.Timeline;


[CreateAssetMenu(fileName = "CaseData", menuName = "CaseData/CaseData")]
public class CaseData : ScriptableObject
{
    public string patientName;
    public string patientAge;
    public string patintSymptoms;
    [Multiline]public string patientDescription;
    public Sprite patientImage;
    [Header("Level index")] public int levelIndex;
    [Header("Pre_Level Timeline")] public TimelineAsset preLevelTimelineAsset;
    [Header("After_Level TimeLine")] public TimelineAsset afterLevelTimelineAsset;
}
