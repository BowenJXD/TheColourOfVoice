using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private Rigidbody2D rb;
    private float inputX;
    private float inputY;
    public bool inDialogueArea = false;

    private Vector2 inputMovement;

    public Transform rotatingTransform;

    public bool fourDirectional;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rotatingTransform) rotatingTransform = transform.GetChild(1);
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
        // if (Input.GetKey(KeyCode.LeftShift))
        // {
        //     inputX *= 0.5f;
        //     inputY *= 0.5f;
        // }

        if (fourDirectional)
        {
            if (Math.Abs(inputX) >= Math.Abs(inputY))
            {
                rb.constraints = (RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation);
            }
            else
            {
                rb.constraints = (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation);
            }
        }

        inputMovement = new Vector2(inputX, inputY).normalized;
    }
    
    private void Movement()
    {
        // rb.velocity = inputMovement * speed;
        Vector2 force = inputMovement * Speed;
        force -= rb.velocity * inputWeight;
        force += inputMovement * (rb.velocity.magnitude * inputWeight);
        rb.AddForce(force);
        Vector2 moveParam = inputMovement == Vector2.zero
            ? Vector2.zero
            : new Vector2(Mathf.Clamp(rb.velocity.x, -1, 1), Mathf.Clamp(rb.velocity.y, -1, 1));
        OnMove?.Invoke(moveParam);
    }

}