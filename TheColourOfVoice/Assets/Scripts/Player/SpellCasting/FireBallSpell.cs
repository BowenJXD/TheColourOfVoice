﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class FireBallSpell : Spell
{
    public float offset;
    
    public AnimEntity animPrefab;
    
    public BulletBase bulletPrefab;

    AnimEntity currentAnim;
    
    BulletBase currentBullet;
    
    void Awake()
    {
        PoolManager.Instance.Register(animPrefab, transform);
        PoolManager.Instance.Register(bulletPrefab);
    }
    
    private void OnDisable()
    {
        if (currentAnim) currentAnim.Deinit();
    }

    public override void StartCasting(CastConfig config)
    {
        base.StartCasting(config);
        currentAnim = PoolManager.Instance.New(animPrefab);
        currentAnim.animDuration = config.chantTime;
        if (upgraded) currentAnim.animDuration *= 2;
        if (currentAnim.TryGetComponent(out DoTweenScale1Behaviour scale1Behaviour))
        {
            scale1Behaviour.scaleSpeed = currentConfig.chantTime;
        }
        currentAnim.transform.position = transform.position;
        currentAnim.transform.localPosition += new Vector3(offset, 0);
        currentAnim.onFinish += EndCasting;
        currentAnim.Init();
    }

    public override void Execute()
    {
        base.Execute();
        currentBullet = PoolManager.Instance.New(bulletPrefab);
        currentBullet.transform.position = currentAnim.transform.position;
        currentBullet.transform.rotation = currentAnim.transform.rotation;
        currentBullet.transform.localScale = currentAnim.transform.localScale;
        Vector3 direction = currentBullet.transform.position - transform.position;
        direction = direction.normalized;

        if (currentBullet.TryGetComponent(out DoTweenScale1Behaviour scale1Behaviour))
        {
            scale1Behaviour.scaleSpeed = currentConfig.chantTime / 2;
        }
        
        currentBullet.Init();
        currentBullet.SetDirection(direction);
        
        currentAnim.Deinit();
    }

    public override void Upgrade()
    {
        base.Upgrade();
        StartCoroutine(LevelManager.Instance.PopUpBubble("BD3"));
    }
}