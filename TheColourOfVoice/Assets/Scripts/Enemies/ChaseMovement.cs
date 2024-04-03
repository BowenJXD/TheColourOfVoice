using System;
using UnityEngine;

public class ChaseMovement : Movement, ISetUp
{
    public Transform target;

    private Rigidbody2D rb;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindWithTag("Player").transform;
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}