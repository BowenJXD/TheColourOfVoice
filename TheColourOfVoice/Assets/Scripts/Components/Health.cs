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
    private Movement movement;
    float inputWeightCache;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        sp = GetComponentInChildren<SpriteRenderer>();
        movement = GetComponent<Movement>();
        if (damageCooldown > 0)
            damageCooldownTask = new LoopTask { interval = damageCooldown, loop = 1, finishAction = ResetCD };
    }
    
    /// <summary>
    /// Reset when disable
    /// </summary>
    public Action<float> TakeDamageAfter;
    
    /// <summary>
    /// Reset when disable
    /// </summary>
    public Action<float> OnHealthChanged;
    
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
        OnHealthChanged?.Invoke(amount);
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
        sp.FlashSprite(new Color(1,1,1,0.2f), damageCooldown);
        if (movement)
        {
            inputWeightCache = movement.inputWeight;
            movement.inputWeight = 0;
        }
    }
    
    void ResetCD()
    {
        invincible = false;
        if (movement) movement.inputWeight += inputWeightCache;
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

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    /// <summary>
    /// Reset when trigger
    /// </summary>
    public Action OnDeath;
}