using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    /// <summary>
    /// Reset when trigger
    /// </summary>
    public Action onInit;

    /// <summary>
    /// Will enable gameObject, need to be called manually
    /// </summary>
    public virtual void Init()
    {
        onInit?.Invoke();
        onInit = null;
        gameObject.SetActive(true);
    }

    /// <summary>
    ///  Reset when trigger
    /// </summary>
    public Action onDeinit;
    
    /// <summary>
    /// Will disable gameObject, need to be called manually
    /// </summary>
    public virtual void Deinit()
    {
        onDeinit?.Invoke();
        onDeinit = null;
    }
}