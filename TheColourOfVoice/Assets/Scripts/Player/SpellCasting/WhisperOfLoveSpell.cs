using UnityEngine;
using UnityEngine.Serialization;

public class WhisperOfLoveSpell : Spell
{
    public int baseDuration = 5;
    public int durationVariation = 5;
    public int healing = 1;
    public int offset = 2;

    public Buff buff;
    public BulletBase bulletPrefab;
    public SpriteRenderer sp;
    
    public override void SetUp()
    {
        base.SetUp();
        if (!buff) buff = GetComponentInChildren<Buff>(true);
        if (buff) buff.Init();
        if (!bulletPrefab) bulletPrefab = GetComponentInChildren<BulletBase>(true);
    }

    public override void StartCasting(CastConfig config)
    {
        base.StartCasting(config);
        if (sp)
        {
            sp.gameObject.SetActive(true);
        }
    }

    public override void Execute()
    {
        base.Execute();
        float duration = baseDuration + durationVariation * (1-currentConfig.peakVolume);
        buff.duration = duration;
        
        var bullet = PoolManager.Instance.New(bulletPrefab);
        Vector3 direction = SpellManager.Instance.transform.rotation * Vector3.right;
        bullet.transform.position = transform.position + direction * offset;
        bullet.GetComponent<Attack>().OnDamage += OnDamage;
        bullet.Init();
        bullet.SetDirection(direction);
        
        if (sp) sp.gameObject.SetActive(false);
    }

    void OnDamage(Health health)
    {
        health.TakeHealing(healing);
        if (health.CompareTag("Enemy"))
        {
            if (health.TryGetComponent(out BuffOwner buffOwner))
            {
                buffOwner.ApplyBuff(buff);
                if (upgraded)
                {
                    if (buffOwner.TryGetComponent(out ChaseMovement chase))
                    {
                        GameObject boss = GameObject.Find("L5-Boss");
                        if (boss) chase.target = boss;
                    }

                    if (buffOwner.TryGetComponentInChildren(out Attack attack))
                    {
                        if (attack.TryGetComponent(out Collider2D col))
                        {
                            col.IncludeLayer(ELayer.Enemy);
                        }
                    }
                }
            }
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        buff.ChangeDuration(999);
        LevelManager.Instance.PopUpBubble("BD4");
    }
}