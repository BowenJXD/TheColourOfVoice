using System;
using UnityEngine;

public class ParticleEntity : Entity
{
    public ParticleSystem ps;
    public bool autoDeinit = true;
    public ParticleSystemStopBehavior stopBehavior; 
    
    public override void SetUp()
    {
        base.SetUp();
        if (!ps) ps = GetComponentInChildren<ParticleSystem>(true);
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
            }, interval = time}.Start();
        }
    }

    private void OnParticleSystemStopped()
    {
        if (!autoDeinit)
        {
            Deinit();
        }
    }
}