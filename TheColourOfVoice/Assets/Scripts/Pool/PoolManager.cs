using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolRegistry
{
    public Type type;
    public EntityPool<Entity> pool;
    public List<Entity> poolList = new List<Entity>();
    public List<Entity> activeList = new List<Entity>();
    
    public List<Entity> GetList(bool activeOnly = true)
    {
        return activeOnly? activeList : poolList;
    }

    public PoolRegistry(Entity prefab, Transform parent = null, int capacity = 100)
    {
        this.type = prefab.GetType();
        pool = new EntityPool<Entity>(prefab, parent, capacity);
    }
    
    public T Get<T>() where T : Entity
    {
        Entity obj = pool.Get();
        if (poolList.Count < pool.TotalCount) poolList.Add(obj);
        activeList.Add(obj);
        return obj as T;
    }
    
    public void Release(Entity obj)
    {
        activeList.Remove(obj);
        pool.Release(obj);
    }
}

/// <summary>
/// Pool Manager that manages all the entity pools' and their entities in the game.
/// To get a pooled entity, call <code>PoolManager.Instance.Get(prefab);</code>
/// Entities would be released to pool upon Deinit. To release a pooled entity, call <code>entity.Deinit();</code> 
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    Dictionary<Entity, PoolRegistry> poolDict = new Dictionary<Entity, PoolRegistry>();
    
    public PoolRegistry Register<T>(T prefab, Transform parent = null, int capacity = 100) where T : Entity
    {
        if (!poolDict.ContainsKey(prefab))
        {
            if (!parent)
            {
                parent = new GameObject($"{prefab.name} Pool").transform;
            }
            poolDict.Add(prefab, new PoolRegistry(prefab, parent, capacity));
        }
        return poolDict[prefab];
    }
    
    public T New<T>(T prefab) where T : Entity
    {
        poolDict.TryGetValue(prefab, out PoolRegistry registry);
        if (registry == null)
        {
            registry = Register(prefab);
        }
        return registry.Get<T>();
    }
    
    public int GetCount<T>(T prefab, bool activeOnly = true) where T : Entity
    {
        poolDict.TryGetValue(prefab, out PoolRegistry registry);
        if (registry == null)
        {
            return 0;
        }
        return registry.GetList(activeOnly).Count;
    }
    
    public List<T> GetAll<T>(T prefab, bool activeOnly = true) where T : Entity
    {
        poolDict.TryGetValue(prefab, out PoolRegistry registry);
        if (registry == null)
        {
            registry = Register(prefab);
        }
        return registry.GetList(activeOnly).ConvertAll(entity => entity as T);
    }

    public List<T> FindAll<T>(Func<Entity, bool> condition = null)
    {
        List<T> result = new ();
        if (condition == null) condition = _ => true;
        foreach (var pool in poolDict.Values)
        {
            result.AddRange(pool.poolList.FindAll(entity => condition(entity)));
        }
        return result;
    }
}