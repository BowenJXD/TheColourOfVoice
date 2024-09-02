using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour, ISetUp
{
    [ReadOnly] public PaintColor color;
    public BulletBase bulletPrefab;
    public float shootingInterval;
    public Rigidbody2D rb;
    public float recoil;
    protected Vector2 mousePos;
    protected Vector2 direction;
    
    LoopTask shootTask;
    
    public ParticleSystem ps;

    /// <summary>
    /// Resets on destroy
    /// </summary>
    public Action<BulletBase> onFire;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        color = LevelManager.Instance.levelColor;
        if (!ps) ps = GetComponentInChildren<ParticleSystem>(true);
        if (ps)
        {
            var main = ps.main;
            var startColor = main.startColor;
            startColor.gradient = ColorManager.Instance.GetGradient(color/*, colorShift.x, colorShift.y, colorShift.z*/);
            main.startColor = startColor;
        }
        SetBullet(bulletPrefab);
        if (bulletPrefab)
        {
            if (bulletPrefab.TryGetComponent(out Painter painter))
            {
                painter.paintColor = color;
            }
        }
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
        float angle = Random.Range(-5f, 5f);
        //GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        var bullet = PoolManager.Instance.New(bulletPrefab);
        bullet.transform.position = transform.position;
        if (color == PaintColor.Rainbow && TryGetComponent(out Painter painter))
        {
            painter.paintColor = ColorManager.Instance.NextRainbow();
        }
        bullet.Init();
        bullet.SetDirection(Quaternion.AngleAxis(angle, Vector3.forward) * direction);
        onFire?.Invoke(bullet);
        if (rb) rb.AddForce(-direction * recoil, ForceMode2D.Impulse);
        
        if (ps)
        {
            if (color == PaintColor.Rainbow)
            {
                var startColor = ps.main.startColor;
                startColor.gradient = ColorManager.Instance.GetGradient(ColorManager.Instance.NextRainbow());
                var main = ps.main;
                main.startColor = startColor;
            }
            ps.Play();
        }
    }

    public void SetInterval(float newInterval)
    {
        shootingInterval = newInterval;
        shootTask.Stop();
        shootTask = new LoopTask
        {
            interval = newInterval,
            loopAction = Shoot,
            loop = -1,
        };
        shootTask.Start();
    }
    
    public void SetBullet(BulletBase newBullet)
    {
        bulletPrefab = newBullet;
        PoolManager.Instance.Register(bulletPrefab);
    }
}
