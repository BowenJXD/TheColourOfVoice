using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

public class ReplicationMechanic : LevelMechanic
{
    public float triggerInterval;
    public float force;
    List<Entity> enemies;
    LoopTask loopTask;
    private StudioEventEmitter emitter;
    
    public override void Init()
    {
        base.Init();
        emitter = GetComponent<StudioEventEmitter>();
        enemies = LevelManager.Instance.entityGenerator.enemyPrefabs;
        loopTask = new LoopTask{loopAction = Replicate, interval = triggerInterval, loop = -1};
        loopTask.Start();
    }
    
    void Replicate()
    {
        foreach (var enemy in enemies)
        {
            var entities = PoolManager.Instance.GetAll(enemy);
            if (entities.Count <= 0) continue;
            
            Entity randomEnemy = entities.GetRandomItem();
            Vector3 spawnPosition = randomEnemy.transform.position;
            Entity newEnemy = LevelManager.Instance.entityGenerator.Spawn(enemy, spawnPosition);
            
            if (force == 0) continue;
            if (!randomEnemy.TryGetComponent(out Rigidbody2D oldRb)) continue;
            Vector3 direction = oldRb.velocity.normalized;
            oldRb.AddForce(direction * force, ForceMode2D.Impulse);
            if (newEnemy.TryGetComponent(out Rigidbody2D newRb))
            {
                newRb.AddForce(-direction * force, ForceMode2D.Impulse);
            }
        }
        if (emitter) emitter.Play();
    }

    public override void Deinit()
    {
        base.Deinit();
        loopTask.Stop();
    }
}