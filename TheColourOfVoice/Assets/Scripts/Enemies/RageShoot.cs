public class RageShoot : RageBehaviour
{
    public float multiplier = 2f;
    ChaseShoot chaseShoot;
    
    public override void SetUp()
    {
        base.SetUp();
        chaseShoot = GetComponent<ChaseShoot>();
    }
    
    protected override void StartRage()
    {
        base.StartRage();
        chaseShoot.SetInterval(chaseShoot.shootInterval / multiplier);
    }
    
    protected override void FinishRage()
    {
        base.FinishRage();
        chaseShoot.SetInterval(chaseShoot.shootInterval * multiplier);
    }
}