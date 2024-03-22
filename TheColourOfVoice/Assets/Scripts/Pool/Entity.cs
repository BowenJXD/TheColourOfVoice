using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    /// <summary>
    /// Reset when trigger
    /// </summary>
    public Action onInit;

    /// <summary>
    /// Trigger from pool, before enabling
    /// </summary>
    public virtual void Init()
    {
        onInit?.Invoke();
        onInit = null;
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