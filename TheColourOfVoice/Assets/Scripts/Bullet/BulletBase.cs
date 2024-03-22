using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : Entity
{
    public float speed;
    private Rigidbody2D rb;
    System.Action<BulletBase> deactiveAction;

    public float duration = 2.0f;
    public LoopTask durationTask;

    public float knockBack = 7.0f;

    [Tooltip("The number of enemies the bullet can penetrate. \n" +
             "0 means it will destroy upon hitting first enemy. \n" +
             "-1 means it can penetrate all enemies.")]
    public int penetration;

    /// <summary>
    /// Avoid hitting the same enemy multiple times.
    /// </summary>
    List<Collider2D> hitEnemies = new List<Collider2D>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        durationTask = new LoopTask
        {
            action = Deinit,
            time = duration,
            loop = 1,
        };
    }

    public override void Init()
    {
        base.Init();
        durationTask.Start();
    }

    public override void Deinit()
    {
        base.Deinit();
        durationTask?.Stop();
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !hitEnemies.Contains(other))
        {
            other.GetComponent<Enemy>().Damage(1, rb.velocity.normalized);
            other.GetComponent<Enemy>().KnockBack(rb.velocity.normalized, knockBack);
            hitEnemies.Add(other);
            if (penetration >= 0 && hitEnemies.Count > penetration)
            {
                hitEnemies.Clear();
                deactiveAction?.Invoke(this);
            }
            //Destroy(gameObject);
        }
    }
}