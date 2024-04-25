using System;
using UnityEngine;

public class AttackApplying : MonoBehaviour, ISetUp
{
    private Attack attack;
    Buff buff;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        if (!attack) attack = GetComponent<Attack>();
        if (!buff) buff = GetComponentInChildren<Buff>(true);
        if (buff) buff.Init();
    }

    private void ApplyBuff(Health health)
    {
        if (health.TryGetComponent(out BuffOwner buffOwner))
        {
            buffOwner.ApplyBuff(buff);
        }
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
        
        if (attack && buff)
        {
            attack.OnDamage += ApplyBuff;
        }
    }
    
    
}