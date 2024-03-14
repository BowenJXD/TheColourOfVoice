using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private float inputX;
    private float inputY;
    public bool isMoving;

    private Vector2 inputMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if (inputX != 0 && inputY != 0) //斜方向速度一致
        {
            inputX *= (float)System.Math.Sqrt(0.5);
            inputY *= (float)System.Math.Sqrt(0.5);
        }
        // if (Input.GetKey(KeyCode.LeftShift))
        // {
        //     inputX *= 0.5f;
        //     inputY *= 0.5f;
        // }

        inputMovement = new Vector2(inputX, inputY);

        isMoving = inputMovement != Vector2.zero;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + inputMovement * speed * Time.fixedDeltaTime);
    }

}