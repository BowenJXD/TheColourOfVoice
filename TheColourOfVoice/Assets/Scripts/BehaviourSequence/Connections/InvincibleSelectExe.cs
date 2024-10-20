public class InvincibleSelectExe : SelectExe
{
    public Health health;

    public override void Init()
    {
        base.Init();
        health = GetComponent<Health>();
    }

    public override void Select()
    {
        selectedExe = health.invincible ? executables[1] : executables[0];
    }
}