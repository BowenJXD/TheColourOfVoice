using System;
using UnityEngine;

public class ChaseBehaviour : MonoBehaviour, ISetUp
{
    public float speed;
    public Transform target;

    private Rigidbody2D rg;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        rg = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rg.velocity = direction * speed;
    }
}