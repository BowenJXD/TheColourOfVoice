using UnityEngine;

public class SpawnHurtParticleExe : ExecutableBehaviour
{
    public ParticleEntity prefab;

    public Painter painter;

    public Transform attacker;
    
    public override void SetUp()
    {
        base.SetUp();
        if (!painter) painter = GetComponent<Painter>();
        if (!attacker) attacker = transform;
    }

    protected override void OnStart()
    {
        base.OnStart();
        ParticleEntity particle = PoolManager.Instance.New(prefab);

        if (!particle.TryGetComponent(out ParticleSystem ps)) return;
        var main = ps.main;
        
        if (blackboard.TryGet(BBKey.TARGET, out Transform target))
        {
            particle.transform.position = target.position;
            Vector2 direction = (target.position - attacker.position).normalized;
            particle.transform.rotation = Quaternion.Euler(0, 0, direction.GetAngle());
            if (painter)
            {
                var mainStartColor = main.startColor;
                mainStartColor.gradient = ColorManager.Instance.GetGradient(painter.paintColor);
                main.startColor = mainStartColor;
            }
        }
        else
        {
            particle.transform.position = transform.position;
        }
        
        particle.Init();
    }
}