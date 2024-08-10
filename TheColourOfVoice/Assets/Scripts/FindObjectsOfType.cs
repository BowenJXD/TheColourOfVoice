using UnityEngine;

public class FindScriptUsage : MonoBehaviour
{
    void Start()
    {
        OpenSpellPanel[] objects = FindObjectsOfType<OpenSpellPanel>();
        foreach (OpenSpellPanel obj in objects)
        {
            Debug.Log("Found object: " + obj.gameObject.name);
        }
    }
}