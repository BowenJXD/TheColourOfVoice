using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootingInterval;
    protected Vector2 mousePos;
    protected Vector2 direction;
    private float timer;

    private void Update()
    {
        //获得鼠标位置的世界坐标
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tryShoot();
    }

    //发射子弹
    void Shoot() 
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;

        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.GetComponent<BulletBase>().SetDirection(Quaternion.AngleAxis(0, Vector3.forward) * direction);
       
    }

    void tryShoot() 
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
