﻿using Cinemachine;
using UnityEngine;

public class Explosion : ParticleEntity
{
    public int particleCount;
    public float angleVariation;
    public float angleOffset;
    public float offsetVariation;
    float[] angles;
    
    public float speed;
    public float speedVariation;
    
    public float duration;
    public float durationVariation;
    
    public PaintColor color;
    public float scale;
    
    public BulletBase bulletPrefab;
    CinemachineImpulseSource impulseSource;

    public override void SetUp()
    {
        base.SetUp();
        IsSet = true;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (ps)
        {
            var main = ps.main;
            var startColor = main.startColor;
            startColor.gradient = ColorManager.Instance.GetGradient(color);
            main.startColor = startColor;
        }
        PoolManager.Instance.Register(bulletPrefab);
        angles = GetAngles();
    }

    public override void Init()
    {
        base.Init();
        Execute();
    }

    public void Execute()
    {
        if (angleVariation > 0) angles =  GetAngles();
        for (int i = 0; i < particleCount; i++)
        {
            BulletBase bullet = PoolManager.Instance.New(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angles[i]);
            bullet.transform.localScale = Vector3.one * scale;
            bullet.speed = speed + Random.Range(-speedVariation, speedVariation);
            bullet.duration = duration + Random.Range(-durationVariation, durationVariation);
            bullet.GetComponent<Painter>()?.SetColor(color);
            bullet.Init();
            bullet.SetDirection(bullet.transform.up);
        }

        if (impulseSource) CameraShakeManager.Instance.CameraShake(impulseSource);
    }
    
    float[] GetAngles()
    {
        angles = new float[particleCount];  
        float offset = angleOffset + Random.Range(-offsetVariation, offsetVariation);
        for (int i = 0; i < particleCount; i++)
        {
            float baseAngle = 360 / particleCount * i + offset;
            float variedAngle = Random.Range(baseAngle - angleVariation, baseAngle + angleVariation);
            angles[i] = variedAngle;
        }
        return angles;
    }
}