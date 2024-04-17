using System;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// The rage mode would decay the health of the object per second
/// </summary>
public class RageBehaviour : MonoBehaviour, ISetUp
{
    [Tooltip("Per second")]
    public float healthDecay;
    [ReadOnly] public bool isRage;

    private Health health;
    
    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        health = GetComponent<Health>();
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }

    public void Ignite()
    {
        if (isRage) return;
        isRage = true;
        StartRage();
    }
    
    protected virtual void StartRage () { }

    private void Update()
    {
        if (!isRage) return;
        if (health && !health.invincible)
        {
            health.AlterHealth(-healthDecay * Time.deltaTime);
        }
    }
    
    private void OnDisable()
    {
        isRage = false;
        FinishRage();
    }

    protected virtual void FinishRage() { }
}