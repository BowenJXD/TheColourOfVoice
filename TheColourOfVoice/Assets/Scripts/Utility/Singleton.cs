using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance) return instance;
            T inst = FindObjectOfType<T>();
            if (inst) return inst;
            inst = Instantiate(new GameObject(typeof(T).Name)).AddComponent<T>();
            if (inst) return inst;
            Debug.LogError("Failed to create instance of " + typeof(T).Name);
            return null;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            instance = this as T;

        }

    }

    protected virtual void OnDestroy()
    {
        if (instance == this) { instance = null; }

    }
}