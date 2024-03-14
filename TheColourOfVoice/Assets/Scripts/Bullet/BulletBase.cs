using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

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
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Bullet")
        {
            ObjectPool.Instance.PushGameObject(gameObject);
        }
    }
}