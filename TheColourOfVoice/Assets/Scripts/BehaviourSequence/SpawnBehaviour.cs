using UnityEngine;

/// <summary>
///  
/// </summary>
public class SpawnBehaviour : BehaviourNode
{
    public Entity prefab;

    protected EntityPool<Entity> pool;

    public override void Init()
    {
        base.Init();
        pool = new EntityPool<Entity>(prefab);
    }

    protected override void OnExecute()
    {
        base.OnExecute();
        Entity ent = pool.Get();
        sequence.Set(BBKey.ENTITY, ent);
        sequence.Set(BBKey.SPAWNER, transform);
    }
}