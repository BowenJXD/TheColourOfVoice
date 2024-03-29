using UnityEngine;

public class ParticleController : Entity
{
    private ParticleSystem ps;
    public bool autoDeinit = true;
    
    public override void SetUp()
    {
        base.SetUp();
        ps = GetComponent<ParticleSystem>();
    }

    public override void Init()
    {
        base.Init();
        ps.Play();
        if (autoDeinit)
        {
            float time = ps.main.duration;
            Invoke(nameof(Deinit), time);
        }
    }
    
    public override void Deinit()
    {
        base.Deinit();
        ps.Stop();
        ps.Clear();
    }
}