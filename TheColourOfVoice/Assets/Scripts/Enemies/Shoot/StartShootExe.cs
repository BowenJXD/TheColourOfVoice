public class StartShootExe : ExecutableBehaviour
{
    public Shooter shooter;

    public override void SetUp()
    {
        base.SetUp();
        shooter = GetComponent<Shooter>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (!shooter) shooter = GetComponent<Shooter>();
        if (shooter) shooter.StartShoot();
    }
}