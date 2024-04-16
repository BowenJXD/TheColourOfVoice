using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigMapMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float inputX;
    private float inputY;
    public bool isMoving;
    public int speed = 10;

    public bool inDialogueArea = false;

    private Vector2 inputMovement;

    private Animator ani;
   

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
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
        //rb.AddForce(inputMovement * speed);
        rb.MovePosition(rb.position + inputMovement * speed * Time.fixedDeltaTime);
        //ani.SetFloat("moveX", Mathf.Clamp(rb.velocity.x, -1, 1));
        //ani.SetFloat("moveY", Mathf.Clamp(rb.velocity.y, -1, 1));
        //ani.SetBool("isMoving", isMoving);
    }
}
