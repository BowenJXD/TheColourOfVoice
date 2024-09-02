using UnityEngine;

public class TargetExe : ExecutableBehaviour
{
    public Transform target;
    
    protected override void OnStart()
    {
        base.OnStart();
        blackboard.Set(BBKey.TARGET, target);
    }
}