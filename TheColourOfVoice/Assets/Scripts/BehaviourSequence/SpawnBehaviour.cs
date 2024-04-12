using UnityEngine;

/// <summary>
///  
/// </summary>
public class SpawnBehaviour : BehaviourNode
{
    public Entity prefab;

    public override void Init()
    {
        base.Init();
        PoolManager.Instance.Register(prefab);
    }

    protected override void OnStart()
    {
        base.OnStart();
        Entity ent = PoolManager.Instance.New(prefab);
        sequence.Set(BBKey.ENTITY, ent);
        sequence.Set(BBKey.SPAWNER, transform);
    }
}