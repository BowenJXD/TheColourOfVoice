using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Pool;

public class Enemy : Entity
{
    private Health health;

    public override void SetUp()
    {
        base.SetUp();
        health = GetComponent<Health>();
    }

    public override void Init()
    {
        base.Init();
        health.OnDeath += Deinit;
    }
}
