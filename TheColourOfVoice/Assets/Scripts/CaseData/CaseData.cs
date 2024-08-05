using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CaseData", menuName = "CaseData/CaseData")]
public class CaseData : ScriptableObject
{
    public string patientName;
    public string patientAge;
    public string patintSymptoms;
    [Multiline]public string patientDescription;
    public Sprite patientImage;
}
