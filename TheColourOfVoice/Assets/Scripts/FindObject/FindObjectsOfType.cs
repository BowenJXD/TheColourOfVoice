using UnityEngine;

public class FindScriptUsage : MonoBehaviour
{
    void Start()
    {
        StarController[] objects = FindObjectsOfType<StarController>();
        foreach (StarController obj in objects)
        {
            Debug.Log("Found object: " + obj.gameObject.name);
        }
    }
}