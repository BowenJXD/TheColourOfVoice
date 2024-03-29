using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletBase : Entity
{
    public float speed;
    private Rigidbody2D rb;

    public float duration = 2.0f;
    public LoopTask durationTask;

    public bool doPenetrate = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        durationTask = new LoopTask
        {
            loopAction = Deinit,
            interval = duration,
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
    
    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!doPenetrate)
            {
                Game.Instance.OnNextUpdate += Deinit;
            }
        }
    }
}