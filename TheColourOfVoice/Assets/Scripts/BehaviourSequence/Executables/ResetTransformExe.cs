using UnityEngine;

public class ResetTransformExe : ExecutableBehaviour
{
    public Transform target;
    
    public bool resetPosition;
    public bool resetRotation;
    public bool resetScale;
    public bool resetParent;
    
    private Vector3 cachedPosition;
    private Quaternion cachedRotation;
    private Vector3 cachedScale;
    private Transform cachedParent;

    public override void SetUp()
    {
        base.SetUp();
        if (!target) target = transform;
        if (resetPosition) cachedPosition = target.position;
        if (resetRotation) cachedRotation = target.rotation;
        if (resetScale) cachedScale = target.localScale;
        if (resetParent) cachedParent = target.parent;
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (resetPosition) target.position = cachedPosition;
        if (resetRotation) target.rotation = cachedRotation;
        if (resetScale) target.localScale = cachedScale;
        if (resetParent) target.parent = cachedParent;
    }
}