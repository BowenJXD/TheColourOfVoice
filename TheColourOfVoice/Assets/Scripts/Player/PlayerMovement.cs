using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private Rigidbody2D rb;
    private float inputX;
    private float inputY;
    public bool isMoving;

    public bool inDialogueArea = false;

    private Vector2 inputMovement;

    private Animator ani;
    private Transform rotatingTransform;

    private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rotatingTransform = transform.GetChild(0);
    }
    private void Update()
    {
        PlayerInput();

        
    }

    private void FixedUpdate()
    {
        Movement();
        
        
        // update rotation to face mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDir = mousePos - rotatingTransform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rotatingTransform.rotation = Quaternion.Euler(0, 0, angle);
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
        rb.AddForce(inputMovement * speed);
        ani.SetFloat("moveX", Mathf.Clamp(rb.velocity.x, -1, 1));
        ani.SetFloat("moveY", Mathf.Clamp(rb.velocity.y, -1, 1));
        ani.SetBool("isMoving", isMoving);
    }

}