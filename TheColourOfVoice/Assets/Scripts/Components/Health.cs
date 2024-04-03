using System;
using Cinemachine;
using UnityEngine;

/// <summary>
///  Health system for game objects, will receive damage from <see cref="Attack"/> component
/// Will also receive knock back and create particle effect when hit
/// </summary>
public class Health : MonoBehaviour, ISetUp
{
    public int maxHealth;
    public int currentHealth;
    
    private CinemachineImpulseSource impulseSource;
    private Rigidbody2D rb;
    private Movement movement;
    private Painter painter;
    private Collider2D col;
    
    public ParticleController hitParticlePrefab;
    public static EntityPool<ParticleController> hitParticlePool;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        rb = GetComponent<Rigidbody2D>();
        IsSet = true;
        hitParticlePool = new EntityPool<ParticleController>(hitParticlePrefab);
        movement = GetComponent<Movement>();
        painter = GetComponent<Painter>();
        col = GetComponent<Collider2D>();
    }
    
    /// <summary>
    /// Reset when disable
    /// </summary>
    public Action<int> TakeDamageAfter;
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        ResetHealth();
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (impulseSource) CameraShakeManager.Instance.CameraShake(impulseSource);
        currentHealth -= damage;
        TakeDamageAfter?.Invoke(damage);
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            OnDeath = null;
        }
    }
    
    public void TakeHealing(int healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeKnockBack(Vector2 direction, float magnitude)
    {
        if(painter) painter.enabled = false;
        if(col) col.enabled = false;
        if (rb) rb.AddForce(magnitude * direction, ForceMode2D.Impulse);
        if (movement) movement.enabled = false;
        ParticleController dust = hitParticlePool.Get();
        dust.transform.SetParent(transform);
        dust.transform.localPosition = new Vector3(0, 0, 0);
        dust.onDeinit += () =>
        {
            if (movement) movement.enabled = true;
            if (painter) painter.enabled = true;
            if(col) col.enabled = true;
        };
        dust.Init();
    }

    private void OnDisable()
    {
        TakeDamageAfter = null;
    }

    /// <summary>
    /// Reset when trigger
    /// </summary>
    public Action OnDeath;
}