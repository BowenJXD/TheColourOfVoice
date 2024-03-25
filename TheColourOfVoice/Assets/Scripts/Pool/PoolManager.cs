using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Pool Manager that manages all the pools' game object in the game.
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<string, GameObject> pools = new();

    public GameObject GetPool(string prefabName)
    {
        if (!pools.ContainsKey(prefabName))
        {
            var pool = new GameObject($"{prefabName} Pool");
            pool.transform.SetParent(transform);
            pools.Add(prefabName, pool);
        }
        
        return pools[prefabName];
    }
}