using System;
using Cinemachine;
using UnityEngine;

/// <summary>
///  Health system for game objects, will receive damage from <see cref="Attack"/> component
/// Will also receive knock back and create particle effect when hit
/// </summary>
public class Health : MonoBehaviour, ISetUp
{
    public float maxHealth;
    public float currentHealth;
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
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>True if the damage is applied.</returns>
    public bool TakeDamage(float damage)
    {
        damage *= (1 - defence);
        if (invincible || damage <= 0) return false;
        
        if (impulseSource) CameraShakeManager.Instance.CameraShake(impulseSource);
        currentHealth -= damage;
        TakeDamageAfter?.Invoke(damage);
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            OnDeath = null;
            return true;
        }
        if (damageCooldown > 0)
        {
            invincible = true;
            damageCooldownTask.Start();
            sp.FlashSprite(Color.clear, damageCooldown);
        }
        return true;
    }
    
    public void TakeHealing(int healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
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