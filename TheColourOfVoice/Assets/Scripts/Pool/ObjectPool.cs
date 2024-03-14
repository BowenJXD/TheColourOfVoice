using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;

    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    /// <summary>
    /// 从对象池中拿到对象,如果没有此对象或者对象池则生成新的对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject GetGameObject(GameObject prefab)
    {
        GameObject obj;
        //如果查找不到此prefab的对象池 或者 此对象池中没有prefab 则建立一个新的对象池
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            obj = GameObject.Instantiate(prefab); //生成新的prefab
            PushGameObject(obj);
            if (pool == null) //如果没有一个父对象池的话 则创建一个
            {
                pool = new GameObject("FatherObjectPool");
            }
            GameObject childPool = GameObject.Find(prefab.name + "Pool"); //找到父对象池下的子对象池
            if (!childPool)//如果找不子对象池 则 创建一个
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform); //将新的子对象池设成父对象的子物体
            }
            obj.transform.SetParent(childPool.transform);
        }
        obj = objectPool[prefab.name].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 向对象池中加入对象
    /// </summary>
    /// <param name="prefab"></param>
    public void PushGameObject(GameObject prefab)
    {
        string poolName = prefab.name.Replace("(Clone)", string.Empty); //将(Clone)替换为空
        if (!objectPool.ContainsKey(poolName))
        {
            objectPool.Add(poolName, new Queue<GameObject>());
        }
        objectPool[poolName].Enqueue(prefab);
        prefab.SetActive(false);
    }
}