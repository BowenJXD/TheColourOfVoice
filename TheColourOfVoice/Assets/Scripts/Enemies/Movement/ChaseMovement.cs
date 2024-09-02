using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChaseMovement : Movement, ISetUp
{
    public GameObject target;
    [Tooltip("The maximum angle to rotate in one second.")]
    public float angularSpeed = 120;
    public float stopRange = 1;
    public float startRange = 1;
    [Tooltip("The variance of speed in percentage (0-1).")]
    public float speedVariance = 0.1f;

    public enum FaceMode
    {
        None,
        Flip,
        RotateUp,
        RotateRight,
        Face,
    }
    public FaceMode faceTarget = FaceMode.Flip;

    /// <summary>
    /// Never reset
    /// </summary>
    public Action OnEnterRange;
    /// <summary>
    /// Never reset
    /// </summary>
    public Action OnExitRange;
    [ReadOnly] public bool isInRange;
    Vector2 lastDirection = Vector2.zero;
    
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    bool initialFlipX;
    
    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponentInChildren<SpriteRenderer>();
        initialFlipX = sp.flipX;
        if (!target) target = FindObjectOfType<ChaseTarget>(true).gameObject;
        if (target) lastDirection = (target.transform.position - transform.position).normalized;
        speed *= Random.Range(1-speedVariance, 1+speedVariance);
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
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (!isInRange && distance < stopRange)
            {
                EnterRange();
            }
            if (isInRange)
            {
                if (distance > startRange) ExitRange();
            }
            
            Vector2 targetDirection = (target.transform.position - transform.position).normalized;
            Vector2 currentDirection = lastDirection;
            float angle = Vector2.SignedAngle(currentDirection, targetDirection);
            float rotateAngle = Mathf.Sign(angle) * Mathf.Min(angularSpeed * Time.fixedDeltaTime, Mathf.Abs(angle));
            newDirection = Util.GetVectorFromAngle(currentDirection.GetAngle() + rotateAngle);
        }
        else if (isInRange)
        {
            ExitRange();
        }
        
        switch (faceTarget)
        {
            case FaceMode.Flip:
                sp.flipX = initialFlipX ? newDirection.x > 0 : newDirection.x < 0;
                break;
            case FaceMode.RotateUp:
                transform.up = newDirection;
                break;
            case FaceMode.RotateRight:
                transform.right = newDirection;
                break;
            case FaceMode.Face:
                transform.LookAt(target.transform);
                break;
        }
        
        if (!isInRange)
        {
            if (Mathf.Abs(Speed) > 0.0001) rb.AddForce(newDirection * Speed);
                // rb.velocity = newDirection * speed;
            OnMove?.Invoke(newDirection);
        }
        lastDirection = newDirection;
    }

    void EnterRange()
    {
        isInRange = true;
        OnEnterRange?.Invoke();
    }
    
    void ExitRange()
    {
        isInRange = false;
        OnExitRange?.Invoke();
    }
    
    private void OnDisable()
    {
    }
}