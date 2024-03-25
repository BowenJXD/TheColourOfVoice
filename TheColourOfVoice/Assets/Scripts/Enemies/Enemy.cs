using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Enemy : Entity
{
    private CinemachineImpulseSource impulseSource;
    private Rigidbody2D rg;
    public ParticleSystem enemyHitTrailSmoke;
    public ParticleSystem enemyDeathExplosion;
    public int maxHealth = 3;
    private int currentHealth;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        rg = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        currentHealth = maxHealth;
    }

    public void Damage(int damageAmount ,Vector3 dir) 
    {
        CameraShakeManager.Instance.CameraShake(impulseSource);
        currentHealth = currentHealth - damageAmount;
        if (currentHealth <= 0)
        {
            ParticleSystem deathExplosion = Instantiate(enemyDeathExplosion, this.transform.position, Quaternion.identity);        
            deathExplosion.Play();
           
            Deinit();
        }
    }

    public void KnockBack(Vector2 direction, float impluse) 
    {
        rg.AddForce(impluse * direction, ForceMode2D.Impulse);
        ParticleSystem dust = Instantiate(enemyHitTrailSmoke,this.transform);
        dust.transform.localPosition = new Vector3(0, 0, 0);
        dust.Play();
        //dust.shape.rotation = Quaternion.identity;
    }

   
}
