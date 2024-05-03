using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DeathExplosion : MonoBehaviour, ISetUp
{
    Entity entity;
    public Explosion prefab;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        entity = GetComponent<Entity>();
        if (!prefab) prefab = GetComponentInChildren<Explosion>(true);
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
        if (entity) entity.onDeinit += ExecuteExplosion;
    }
    
    void ExecuteExplosion()
    {
        Explosion exp = PoolManager.Instance.New(prefab);
        exp.transform.position = transform.position;
        exp.color = LevelManager.Instance.levelColor;
        exp.Init();
    }
}