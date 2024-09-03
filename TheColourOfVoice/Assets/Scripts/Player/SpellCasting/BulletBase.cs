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

    public bool useRotation = false;
    public bool doPenetrate = false;

    public bool fourDirectional = false;
    
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
        if (useRotation)
        {
            rb.velocity = transform.right * speed;
        }
    }

    public override void Deinit()
    {
        base.Deinit();
        durationTask?.Stop();
    }

    public void SetDirection(Vector2 direction)
    {
        if (fourDirectional)
        {
            direction = Util.GetNearestFourDirection(direction);
        }
        
        transform.rotation = Quaternion.Euler(0, 0, direction.GetAngle());
        rb.velocity = direction * speed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            if (!doPenetrate)
            {
                Game.Instance.OnNextUpdate += Deinit;
            }
        }
    }
}