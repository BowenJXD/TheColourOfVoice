using System;
using UnityEngine;

public class DirectionalMovement : Movement, ISetUp
{
    [Tooltip("in degrees, 0 is right, 90 is up, 180 is left, 270 is down")]
    public int direction;

    public bool goToCenter;
    
    Vector2 directionVector;
    
    public Shooter shooter;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        if (!shooter) shooter = GetComponent<Shooter>();
    }
    
    void OnEnable()
    {
        if (!IsSet) SetUp();
        if (goToCenter)
        {
            // decide whether to go up, down, left or right depending on the current position and 0,0
            var pos = transform.position;
            if (Math.Abs(pos.x) >= Math.Abs(pos.y))
            {
                direction = pos.x > 0 ? 180 : 0;
                if (shooter) shooter.angle = pos.y > 0 ? 270 : 90;
            }
            else
            {
                direction = pos.y > 0 ? 270 : 90;
                if (shooter) shooter.angle = pos.x > 0 ? 180 : 0;
            }
        }
        
        directionVector = Util.GetVectorFromAngle(direction);
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