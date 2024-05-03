using System;
using UnityEngine;

public class CalmingLaserSpell : Spell
{
    public float offset;
    
    public AnimEntity animPrefab;
    
    public BulletBase bulletPrefab;

    AnimEntity currentAnim;
    private Animator ani;
    
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
        currentAnim.transform.position = transform.position;
        currentAnim.transform.localPosition += new Vector3(offset, 0);
        currentAnim.onFinish += EndCasting;
        ani = currentAnim.GetComponentInChildren<Animator>();
        if (ani)
        {
            ani.SetBool("Release", false);
            ani.SetFloat("ChantingSpeed", 1 / config.chantTime);
        }
        currentAnim.Init();
    }

    public override void Execute()
    {
        base.Execute();
        currentBullet = PoolManager.Instance.New(bulletPrefab);
        currentBullet.transform.position = currentAnim.transform.position;
        currentBullet.transform.rotation = currentAnim.transform.rotation;
        Vector3 direction = currentBullet.transform.position - transform.position;
        direction = direction.normalized;
        direction *= currentConfig.chantTime;
        
        if (currentBullet.TryGetComponent(out Attack attack))
        {
            attack.OnDamage += OnDamage;
        }
        
        currentBullet.Init();
        currentBullet.SetDirection(direction);
        
        if (ani) ani.SetBool("Release", true);
        new LoopTask{interval = 1, finishAction = currentAnim.Deinit}.Start();
    }

    private void OnDamage(Health obj)
    {
        if (obj.TryGetComponent(out RageBehaviour rage))
        {
            rage.Extinguish();
        }
    }
}