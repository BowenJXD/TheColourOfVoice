using UnityEngine;
using UnityEngine.Pool;

public class CustomPool<T> : ObjectPool<T> where T : Component
{
    public T prefab;
    
    public CustomPool(T defaultPrefab, Transform parent = null, int defaultCapacity = 100) :
        base(() =>
            {
                T instance = parent ? Object.Instantiate(defaultPrefab, parent) : Object.Instantiate(defaultPrefab);
                return instance;
            }, prefab =>
            {
                prefab.transform.SetParent(parent);
            }
            , prefab =>
            {
                prefab.gameObject.SetActive(false);
            }
            , prefab =>
            {
                Object.Destroy(prefab);
            }
            , true, defaultCapacity)
    {
        prefab = defaultPrefab;
            
    }
}