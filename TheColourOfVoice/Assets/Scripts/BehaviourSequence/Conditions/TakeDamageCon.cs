public class TakeDamageCon : ConditionBehaviour
{
    private Health health;

    public override void SetUp()
    {
        base.SetUp();
        health = GetComponent<Health>();
    }
    
    protected override void Init()
    {
        health.TakeDamageAfter += _ => StartCoroutine(Execute());
    }
    
    protected override void Deinit()
    {
        health.TakeDamageAfter -= _ => StartCoroutine(Execute());
    }
}