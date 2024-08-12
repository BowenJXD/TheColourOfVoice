using UnityEngine;

public class FindScriptUsage : MonoBehaviour
{
    void Start()
    {
        PausePanel[] objects = FindObjectsOfType<PausePanel>();
        foreach (PausePanel obj in objects)
        {
            Debug.Log("Found object: " + obj.gameObject.name);
        }
    }
}