using System;
using UnityEngine;

public class DirectionalMovement : Movement
{
    [Tooltip("in degrees, 0 is right, 90 is up, 180 is left, 270 is down")]
    public float direction;

    public bool goToCenter;
    
    Vector2 directionVector;

    private void Start()
    {
        if (goToCenter)
        {
            // decide whether to go up, down, left or right depending on the current position and 0,0
            Vector2 center = Vector2.zero;
            direction = Vector2.SignedAngle(Vector2.right, center - (Vector2)transform.position);
            direction = Mathf.Round(direction / 90) * 90;
        }
        
        directionVector = new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad));
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector2 moveVector = directionVector * (Speed * Time.deltaTime);
        transform.Translate(moveVector);
        OnMove?.Invoke(moveVector);
    }
}