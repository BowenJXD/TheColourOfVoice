using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BuffOwner : MonoBehaviour
{
    public Dictionary<string, Buff> buffs = new();
    
    public List<string> resistingBuffs = new();

    public bool ApplyBuff(Buff buffPrefab)
    {
        string buffName = buffPrefab.GetType().Name;
        if (resistingBuffs.Contains(buffName)) return false;
        if (!buffs.ContainsKey(buffName))
        {
            Buff buff = buffPrefab.buffPool.Get();
            buff.transform.SetParent(transform);
            buff.buffPool = buffPrefab.buffPool;
            buffs.Add(buffName, buff);
            buff.OnApply(this);
        }
        else if (buffPrefab.isStackable)
        {
            buffs[buffName].OnStack();
        }
        else
        {
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Should only be called by buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        buffs.Remove(buff.GetType().Name);
        buff.OnRemove();
    }
}