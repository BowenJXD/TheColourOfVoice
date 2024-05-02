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
    private SpriteRenderer sp;
    private Animator ani;
    Sprite defaultSprite;
    
    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        health = GetComponent<Health>();
        sp = GetComponentInChildren<SpriteRenderer>();
        ani = GetComponent<Animator>();
        ani.enabled = isRage;
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

    protected virtual void StartRage()
    {
        if (ani)
        {
            ani.enabled = true;
        }

        if (sp)
        {
            defaultSprite = sp.sprite;
        }
    }

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
        if (isRage) FinishRage();
        isRage = false;
    }

    public void Extinguish()
    {
        if (!isRage) return;
        isRage = false;
        FinishRage();
    }
    
    protected virtual void FinishRage()
    {
        if (sp && ani)
        {
            ani.enabled = false;
            sp.sprite = defaultSprite;
        }
    }
}