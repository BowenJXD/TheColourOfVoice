using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour, ISetUp
{
    public BulletBase bulletPrefab;
    public float shootingInterval;
    public Rigidbody2D rb;
    public float recoil;
    protected Vector2 mousePos;
    protected Vector2 direction;
    
    LoopTask shootTask;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        PoolManager.Instance.Register(bulletPrefab);
        shootTask = new LoopTask
        {
            interval = shootingInterval,
            loopAction = Shoot,
            loop = -1,
        };
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
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
        var bullet = PoolManager.Instance.New(bulletPrefab);
        bullet.transform.position = this.transform.position;
        bullet.Init();
        bullet.SetDirection(Quaternion.AngleAxis(angel, Vector3.forward) * direction);
        if (rb) rb.AddForce(-direction * recoil, ForceMode2D.Impulse);
    }
}
