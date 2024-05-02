using System;
using UnityEngine;

public class KeepParticleOnDeinit : MonoBehaviour, ISetUp
{
    public Entity entity;
    public ParticleSystem ps;
    
    Transform cachedParent;
    
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        if (!entity) entity = GetComponentInParent<Entity>();
        if (!ps) ps = GetComponent<ParticleSystem>();
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        entity.onDeinit += Keep;
    }

    private void Keep()
    {
        cachedParent = ps.transform.parent;
        ps.transform.SetParent(null);
        // ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        
        var main = ps.main;
        float duration = Mathf.Max(main.startLifetime.constantMax, main.startLifetime.constant);
        new LoopTask
        {
            loopAction = () =>
            {
                ps.transform.SetParent(cachedParent);
            },
            interval = duration,
        }.Start();
    }
}