﻿using System;
using UnityEngine;

/// <summary>
/// Attack system for game objects, will deal damage to <see cref="Health"/> component when colliding
/// Will also apply knockback to the target
/// </summary>
public class Attack : MonoBehaviour, ISetUp
{
    public int damage = 1;
    public float cooldown = float.MaxValue;
    public bool resetCDOnExit = true;
    public float knockBack = 7.0f;

    LoopTask loopTask;

    private Health target;
    private Rigidbody2D rb;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        
        rb = GetComponent<Rigidbody2D>();
        
        loopTask = new LoopTask { interval = cooldown, loop = -1, loopAction = Damage };
        if (!resetCDOnExit) loopTask.Start();
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            target = health;
            StartAttack();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        target = null;
        StopAttack();
    }

    void StartAttack()
    {
        if (resetCDOnExit)
        {
            Damage();
            loopTask.Start();
        }
    }
    
    void StopAttack()
    {
        if (resetCDOnExit)
        {
            loopTask.Stop();
        }
    }

    void Damage()
    {
        if (!target) return;
        target.TakeDamage(damage);
        if (!target) return;
        Vector3 direction = rb? rb.velocity.normalized : transform.rotation.eulerAngles;
        target.TakeKnockBack(direction, knockBack);
    }

}