using System;
using UnityEngine;

public class ChaseMovement : Movement, ISetUp
{
    [Tooltip("The maximum angle to rotate in one second.")]
    public float angularSpeed = 120;
    public Transform target;
    public Vector2 lastDirection = Vector2.zero;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    bool initialFlipX;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponentInChildren<SpriteRenderer>();
        initialFlipX = sp.flipX;
        if (!target) target = GameObject.FindWithTag("Player").transform;
        if (target) lastDirection = (target.position - transform.position).normalized * speed;
        
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 targetDirection = (target.position - transform.position).normalized;
        Vector2 currentDirection = lastDirection;
        float angle = Vector2.SignedAngle(currentDirection, targetDirection);
        float rotateAngle = Mathf.Sign(angle) * Mathf.Min(angularSpeed * Time.fixedDeltaTime, Mathf.Abs(angle));
        Vector2 newDirection = Util.GetVectorFromAngle(currentDirection.GetAngle() + rotateAngle);
        rb.velocity = newDirection * speed;
        sp.flipX = initialFlipX ? rb.velocity.x > 0 : rb.velocity.x < 0;
        lastDirection = newDirection;
    }
}