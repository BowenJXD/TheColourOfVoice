using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadSetter : MonoBehaviour
{
    public List<GameObject> objectsToPersist;
    
    private void Awake()
    {
        foreach (var obj in objectsToPersist)
        {
            DontDestroyOnLoad(obj);
        }
    }
}