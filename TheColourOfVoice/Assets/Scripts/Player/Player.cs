public class Player : Entity
{
    public override void SetUp()
    {
        base.SetUp();
        Health health = GetComponent<Health>();
        health.OnDeath += Deinit;
    }
    
    
}