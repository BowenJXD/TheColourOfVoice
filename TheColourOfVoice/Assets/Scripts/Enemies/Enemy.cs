using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Pool;

public class Enemy : Entity
{
    private CinemachineImpulseSource impulseSource;
    private Rigidbody2D rg;
    public int maxHealth = 3;
    private int currentHealth;
    
    public ParticleController deathParticlePrefab;
    public static EntityPool<ParticleController> deathParticlePool;
    
    public ParticleController hitParticlePrefab;
    public static EntityPool<ParticleController> hitParticlePool;
    
    ChaseBehaviour chaseBehaviour;

    public override void SetUp()
    {
        base.SetUp();
        deathParticlePool = new EntityPool<ParticleController>(deathParticlePrefab);
        hitParticlePool = new EntityPool<ParticleController>(hitParticlePrefab);
        chaseBehaviour = GetComponent<ChaseBehaviour>();
    }

    public override void Init()
    {
        base.Init();
        rg = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        currentHealth = maxHealth;
    }

    public void Damage(int damageAmount) 
    {
        CameraShakeManager.Instance.CameraShake(impulseSource);
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Deinit();
        }
    }

    public override void Deinit()
    {
        base.Deinit();
        var fx = deathParticlePool.Get();
        fx.transform.position = transform.position;
        fx.transform.rotation = Quaternion.identity;
        fx.Init();
    }

    public void KnockBack(Vector2 direction, float impulse) 
    {
        rg.AddForce(impulse * direction, ForceMode2D.Impulse);
        if (chaseBehaviour) chaseBehaviour.enabled = false;
        ParticleController dust = hitParticlePool.Get();
        dust.transform.SetParent(transform);
        dust.transform.localPosition = new Vector3(0, 0, 0);
        dust.onDeinit += () =>
        {
            if (chaseBehaviour) chaseBehaviour.enabled = true;
        };
        dust.Init();
        //dust.shape.rotation = Quaternion.identity;
    }
}
