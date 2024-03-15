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
            instance = FindObjectOfType<T>();
            if (instance) return instance;
            instance = Instantiate(new GameObject(typeof(T).Name)).AddComponent<T>();
            if (instance) return instance;
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