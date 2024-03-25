using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour
{
    public BulletBase bulletPrefab;
    public float shootingInterval;
    protected Vector2 mousePos;
    protected Vector2 direction;
    
    LoopTask shootTask;
    
    EntityPool<BulletBase> pool;

    private void Awake()
    {
        pool = new EntityPool<BulletBase>(bulletPrefab);

        shootTask = new LoopTask
        {
            duration = shootingInterval,
            loopAction = Shoot,
            loop = -1,
        };
    }

    private void OnEnable()
    {
        shootTask.Start();
    }

    private void OnDisable()
    {
        shootTask.Stop();
    }

    private void Update()
    {
        //获得鼠标位置的世界坐标
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //发射子弹
    void Shoot() 
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        float angel = Random.Range(-5f, 5f);
        //GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        var bullet = pool.Get();
        bullet.transform.position = this.transform.position;
        bullet.Init();
        bullet.SetDirection(Quaternion.AngleAxis(angel, Vector3.forward) * direction);
    }
}
