using System;
using UnityEngine;

/// <summary>
/// Animate <see cref="Movement"/> using <see cref="Animator"/>
/// </summary>
public class MovementAnim : MonoBehaviour, ISetUp
{
    Movement movement;
    private Animator ani;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        movement = GetComponent<Movement>();
        ani = GetComponentInChildren<Animator>(true);
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
        movement.OnMove += OnMove;
    }

    void OnMove(Vector2 direction)
    {
        if (!ani) return;
        ani.SetFloat("moveX", direction.x);
        ani.SetFloat("moveY", direction.y);
        ani.SetBool("isMoving", direction != Vector2.zero);
    }
    
    private void OnDisable()
    {
        movement.OnMove -= OnMove;
    }
}