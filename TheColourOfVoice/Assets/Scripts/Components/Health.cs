using System;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
///  Health system for game objects, will receive damage from <see cref="Attack"/> component
/// Will also receive knock back and create particle effect when hit
/// </summary>
public class Health : MonoBehaviour, ISetUp
{
    public float maxHealth;
    [ReadOnly] public float currentHealth;
    [Range(0, 1)]
    [Tooltip("Percentage of damage reduced when taking damage")]
    public float defence;
    public bool invincible;
    public float damageCooldown;
    LoopTask damageCooldownTask;

    private SpriteRenderer sp;
    private CinemachineImpulseSource impulseSource;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        sp = GetComponentInChildren<SpriteRenderer>();
        if (damageCooldown > 0)
            damageCooldownTask = new LoopTask { interval = damageCooldown, loop = 1, finishAction = ResetCD };
    }
    
    /// <summary>
    /// Reset when disable
    /// </summary>
    public Action<float> TakeDamageAfter;
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        ResetHealth();
    }

    /// <summary>
    /// Change the health directly (without damage nor healing)
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>False if reaches 0 or the max.</returns>
    public bool AlterHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            OnDeath = null;
            return false;
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>True if the damage is applied.</returns>
    public bool TakeDamage(float damage)
    {
        damage *= (1 - defence);
        if (invincible || damage <= 0) return false;
        
        if (impulseSource) CameraShakeManager.Instance.CameraShake(impulseSource);
        bool isAlive = AlterHealth(-damage);
        TakeDamageAfter?.Invoke(damage);
        if (isAlive && damageCooldown > 0)
        {
            StartCD();
        }
        return true;
    }
    
    public void TakeHealing(int healing)
    {
        AlterHealth(healing);
    }

    public void StartCD()
    {
        invincible = true;
        damageCooldownTask.Start();
        sp.FlashSprite(Color.clear, damageCooldown);
    }
    
    void ResetCD()
    {
        invincible = false;
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void OnDisable()
    {
        TakeDamageAfter = null;
        if (damageCooldownTask.isPlaying)
        {
            damageCooldownTask.Stop();
            ResetCD();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }



    /// <summary>
    /// Reset when trigger
    /// </summary>
    public Action OnDeath;
}