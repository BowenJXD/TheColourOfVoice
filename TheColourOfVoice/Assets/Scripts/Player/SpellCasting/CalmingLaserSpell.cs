using UnityEngine;

public class CalmingLaserSpell : Spell
{
    public float offset;
    
    public AnimEntity animPrefab;
    
    public BulletBase bulletPrefab;

    EntityPool<AnimEntity> animPool;

    EntityPool<BulletBase> bulletPool;
    
    AnimEntity currentAnim;
    
    BulletBase currentBullet;
    
    void Awake()
    {
        animPool = new EntityPool<AnimEntity>(animPrefab, transform);
        bulletPool = new EntityPool<BulletBase>(bulletPrefab);
    }

    public override void StartCasting(CastConfig config)
    {
        base.StartCasting(config);
        currentAnim = animPool.Get();
        currentAnim.animDuration = config.chantTime;
        currentAnim.transform.position = transform.position;
        currentAnim.transform.localPosition += new Vector3(offset, 0);
        currentAnim.onFinish += EndCasting;
        currentAnim.Init();
    }

    public override void Execute()
    {
        base.Execute();
        currentBullet = bulletPool.Get();
        currentBullet.transform.position = currentAnim.transform.position;
        currentBullet.transform.rotation = currentAnim.transform.rotation;
        Vector3 direction = currentBullet.transform.position - transform.position;
        direction = direction.normalized;
        direction *= currentConfig.chantTime;
        currentBullet.Init();
        currentBullet.SetDirection(direction);
        
        currentAnim.Deinit();
    }
}