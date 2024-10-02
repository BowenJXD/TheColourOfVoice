using UnityEngine;

public class TargetExe : ExecutableBehaviour
{
    public Transform target;
    public bool useChaseTarget = false;

    public override void Init()
    {
        base.Init();
        if (useChaseTarget)
        {
            target = FindObjectOfType<ChaseTarget>().transform;
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        blackboard.Set(BBKey.TARGET, target);
    }
}