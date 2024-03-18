using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    System.Action<BulletBase> deactiveAction;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {            
            other.GetComponent<Enemy>().Damage(1, rb.velocity.normalized);
            other.GetComponent<Enemy>().KnockBack(rb.velocity.normalized,7.0f);
            this.deactiveAction?.Invoke(this);
            //Destroy(gameObject);
        }
    }

    public void SetDeactiveAction(System.Action<BulletBase> deactiveAction) 
    {
        this.deactiveAction = deactiveAction;
    }
}