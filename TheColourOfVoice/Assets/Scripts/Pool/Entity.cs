using System;
using UnityEngine;

public class Entity : MonoBehaviour, ISetUp
{
    public bool IsSet { get; set; }

    /// <summary>
    /// Called when the game starts, despite if the game object is active or not
    /// </summary>
    public virtual void SetUp()
    {
        IsSet = true;
    }
    
    /// <summary>
    /// Reset when trigger
    /// </summary>
    public Action onInit;

    /// <summary>
    /// Will enable gameObject, need to be called manually
    /// </summary>
    public virtual void Init()
    {
        if (!IsSet) SetUp();
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