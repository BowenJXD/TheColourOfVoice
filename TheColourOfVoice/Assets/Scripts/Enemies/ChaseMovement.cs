using System;
using UnityEngine;

public class ChaseMovement : Movement, ISetUp
{
    [Tooltip("The maximum angle to rotate in one second.")]
    public float angularSpeed = 120;
    public GameObject target;
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
        if (!target) target = FindObjectOfType<ChaseTarget>(true).gameObject;
        if (target) lastDirection = (target.transform.position - transform.position).normalized;
        
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    private void FixedUpdate()
    {
        Vector2 newDirection = lastDirection;
        if (target && target.activeSelf)
        {
            Vector2 targetDirection = (target.transform.position - transform.position).normalized;
            Vector2 currentDirection = lastDirection;
            float angle = Vector2.SignedAngle(currentDirection, targetDirection);
            float rotateAngle = Mathf.Sign(angle) * Mathf.Min(angularSpeed * Time.fixedDeltaTime, Mathf.Abs(angle));
            newDirection = Util.GetVectorFromAngle(currentDirection.GetAngle() + rotateAngle);
        }

        rb.AddForce(newDirection * speed);
        sp.flipX = initialFlipX ? rb.velocity.x > 0 : rb.velocity.x < 0;
        lastDirection = newDirection;
    }
}