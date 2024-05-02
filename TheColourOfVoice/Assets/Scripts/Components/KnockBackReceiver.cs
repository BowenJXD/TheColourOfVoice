using System;
using Cinemachine;
using UnityEngine;

public class KnockBackReceiver : MonoBehaviour, ISetUp
{
    private Rigidbody2D rb;
    private Movement movement;
    private Painter painter;
    private Collider2D col;
    private Health health;
    public bool invincible = true;
    public bool disableMovement = true;
    public bool disablePainter = true;
    public bool disableCol = true;

    public ParticleController hitParticlePrefab;
    private CinemachineImpulseSource impulseSource;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        rb = GetComponent<Rigidbody2D>();
        PoolManager.Instance.Register(hitParticlePrefab);
        movement = GetComponent<Movement>();
        painter = GetComponent<Painter>();
        col = GetComponent<Collider2D>();
        health = GetComponent<Health>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    public void TakeKnockBack(Vector2 direction, float magnitude)
    {
        ParticleController dust = PoolManager.Instance.New(hitParticlePrefab);
        dust.transform.SetParent(transform);
        dust.transform.localPosition = Vector3.zero;
        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction * magnitude, ForceMode2D.Impulse);
            dust.onDeinit += () => rb.velocity = Vector2.zero;
        }
        if (disableMovement && movement && movement.enabled)
        {
            movement.enabled = false;
            dust.onDeinit += () => movement.enabled = true;
        }
        if (invincible && health && health.invincible == false)
        {
            health.invincible = true;
            dust.onDeinit += () => health.invincible = false;
        }
        if (disablePainter && painter && painter.enabled)
        {
            painter.enabled = false;
            dust.onDeinit += () => painter.enabled = true;
        }
        if (disableCol && col && col.enabled)
        {
            LayerMask inCache = col.includeLayers;
            LayerMask exCache = col.excludeLayers;
            col.ExcludeAllLayers(ELayer.Bound);
            dust.onDeinit += () =>
            {
                col.includeLayers = inCache;
                col.excludeLayers = exCache;
            };
        }
        dust.Init();
    }
}