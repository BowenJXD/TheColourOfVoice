using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Pool;

public class Enemy : Entity
{
    public ParticleController deathParticlePrefab;
    public static EntityPool<ParticleController> deathParticlePool;

    public override void SetUp()
    {
        base.SetUp();
        deathParticlePool = new EntityPool<ParticleController>(deathParticlePrefab);
        Health health = GetComponent<Health>();
        health.OnDeath += Deinit;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Deinit()
    {
        base.Deinit();
        var fx = deathParticlePool.Get();
        fx.transform.position = transform.position;
        fx.transform.rotation = Quaternion.identity;
        fx.Init();
    }
}
