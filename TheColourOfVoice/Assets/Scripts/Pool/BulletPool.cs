using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : BasePool<BulletBase>
{
    
    private void Awake()
    {
        Init();
    }

    protected override BulletBase OnCreatePoolItem()
    {
        var bullet = base.OnCreatePoolItem();
        bullet.SetDeactiveAction(delegate { Release(bullet); });

        return bullet;
    }

    protected override void OnGetPoolItem(BulletBase obj)
    {
        base.OnGetPoolItem(obj);
       
    }

    public void SetPrefab(BulletBase bullet) 
    {
        this.prefab = bullet;
    }
}
