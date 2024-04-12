using UnityEngine;

public class CalmingLaserSpell : Spell
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

    public override void StartCasting(CastConfig config)
    {
        base.StartCasting(config);
        currentAnim = PoolManager.Instance.New(animPrefab);
        currentAnim.animDuration = config.chantTime;
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
        Vector3 direction = currentBullet.transform.position - transform.position;
        direction = direction.normalized;
        direction *= currentConfig.chantTime;
        currentBullet.Init();
        currentBullet.SetDirection(direction);
        
        currentAnim.Deinit();
    }
}