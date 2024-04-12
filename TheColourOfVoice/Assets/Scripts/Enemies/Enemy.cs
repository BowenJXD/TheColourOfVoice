using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Pool;

public class Enemy : Entity
{
    public ParticleController deathParticlePrefab;
    
    private Health health;

    public override void SetUp()
    {
        base.SetUp();
        PoolManager.Instance.Register(deathParticlePrefab);
        health = GetComponent<Health>();
    }

    public override void Init()
    {
        base.Init();
        health.OnDeath += Deinit;
    }

    public override void Deinit()
    {
        base.Deinit();
        var fx = PoolManager.Instance.New(deathParticlePrefab);
        fx.transform.position = transform.position;
        fx.transform.rotation = Quaternion.identity;
        fx.Init();
    }
}
