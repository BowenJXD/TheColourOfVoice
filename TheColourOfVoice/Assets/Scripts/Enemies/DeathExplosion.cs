using System;
using UnityEngine;

public class DeathExplosion : MonoBehaviour, ISetUp
{
    Entity entity;
    Explosion explosion;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        entity = GetComponent<Entity>();
        explosion = GetComponent<Explosion>();
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
        if (entity) entity.onDeinit += ExecuteExplosion;
    }
    
    void ExecuteExplosion()
    {
        if (explosion) explosion.Execute();
    }
}