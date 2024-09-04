using UnityEngine;

public class SetAnglesToTargetExe : ExecutableBehaviour
{
    protected override void OnStart()
    {
        base.OnStart();
        if (blackboard.TryGet(BBKey.TARGET, out Transform target))
        {
            blackboard.Set(BBKey.BASE_ANGLE, Util.GetAngle(target.transform.position - transform.position));
        }
    }
}