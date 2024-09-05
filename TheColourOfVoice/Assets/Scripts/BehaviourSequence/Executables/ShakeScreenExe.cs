using Cinemachine;

public class ShakeScreenExe : ExecutableBehaviour
{
    CinemachineImpulseSource impulseSource;

    public override void Init()
    {
        base.Init();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (impulseSource) CameraShakeManager.Instance.CameraShake(impulseSource);
    }
}