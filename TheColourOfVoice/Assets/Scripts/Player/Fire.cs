using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public BulletBase bulletPrefab;
    public float shootingInterval;
    protected Vector2 mousePos;
    protected Vector2 direction;
    private float timer;

    BulletPool pool;

    private void Start()
    {
        var poolHolder = new GameObject($"Pool:{ bulletPrefab.name}");
        poolHolder.transform.parent = transform;
        poolHolder.transform.position = transform.position;
        poolHolder.SetActive(false);
        pool = poolHolder.AddComponent<BulletPool>();
        pool.SetPrefab(bulletPrefab);
        poolHolder.SetActive(true);

    }
    private void Update()
    {
        //获得鼠标位置的世界坐标
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TryShoot();
    }

    //发射子弹
    void Shoot() 
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        float angel = Random.Range(-5f, 5f);
        //GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        var bullet = pool.Get();
        bullet.transform.position = this.transform.position;
        bullet.SetDirection(Quaternion.AngleAxis(angel, Vector3.forward) * direction);
    }

    void TryShoot() 
    {
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (timer == 0)
            {
                timer = shootingInterval;
                Shoot();

            }
        }
    }
}
