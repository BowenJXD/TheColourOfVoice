using UnityEngine;

public class FindScriptUsage : MonoBehaviour
{
    void Start()
    {
        Level_demo[] objects = FindObjectsOfType<Level_demo>();
        foreach (Level_demo obj in objects)
        {
            Debug.Log("Found object: " + obj.gameObject.name);
        }
    }
}