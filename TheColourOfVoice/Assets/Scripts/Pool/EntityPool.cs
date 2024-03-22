using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class EntityPool<T> where T : Entity
{
    protected T prefab;
    int defaultSize = 100;
    int maxSize = 500;

    Transform parent;
    ObjectPool<T> pool;

    public int ActiveCount => pool.CountActive;
    public int InactiveCount => pool.CountInactive;
    public int TotalCount => pool.CountAll;

    public EntityPool(T prefab, Transform parent = null, int defaultSize = 100, int maxSize = 500)
    {
        this.prefab = prefab;
        this.defaultSize = defaultSize;
        this.maxSize = maxSize;
        this.parent = parent ? parent : PoolManager.Instance.GetPool(prefab.name).transform;
        Init();
    }

    protected void Init(bool collectionCheck = true) =>
        pool = new ObjectPool<T>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, collectionCheck, defaultSize, maxSize);

    protected virtual T OnCreatePoolItem() => Object.Instantiate(prefab, parent);
    protected virtual void OnGetPoolItem(T obj)
    {
        obj.transform.SetParent(parent);
        obj.Init();
        obj.gameObject.SetActive(true);
        obj.onDeinit += () => Release(obj);
    }
    protected virtual void OnReleasePoolItem(T obj) => obj.gameObject.SetActive(false);
    protected virtual void OnDestroyPoolItem(T obj) => Object.Destroy(obj.gameObject);

    public T Get() => pool.Get();
    public void Release(T obj) => pool.Release(obj);
    public void Clear() => pool.Clear();
}