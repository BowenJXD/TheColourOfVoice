using System;
using UnityEngine;

public class ParticleEntity : Entity
{
    protected ParticleSystem ps;
    public bool autoDeinit = true;
    public ParticleSystemStopBehavior stopBehavior; 
    
    public override void SetUp()
    {
        base.SetUp();
        ps = GetComponentInChildren<ParticleSystem>(true);
    }

    public override void Init()
    {
        base.Init();
        ps.Play();
        if (autoDeinit)
        {
            float time = ps.main.duration;
            new LoopTask{finishAction = () =>
            {
                Deinit();
                ps.Stop(false, stopBehavior);
            }}.Start();
        }
    }

    private void OnParticleSystemStopped()
    {
        Deinit();
    }
}