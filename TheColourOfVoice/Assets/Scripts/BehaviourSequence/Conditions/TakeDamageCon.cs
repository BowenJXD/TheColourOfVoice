public class TakeDamageCon : ConditionBehaviour
{
    public bool isBeforeDamage = false;
    private Health health;

    public override void SetUp()
    {
        base.SetUp();
        health = GetComponent<Health>();
    }
    
    protected override void Init()
    {
        if (isBeforeDamage) 
            health.OnTakeDamage += _ => StartCoroutine(Execute());
        else
            health.TakeDamageAfter += _ => StartCoroutine(Execute());
    }
    
    protected override void Deinit()
    {
        if (isBeforeDamage) 
            health.OnTakeDamage -= _ => StartCoroutine(Execute());
        else
            health.TakeDamageAfter -= _ => StartCoroutine(Execute());
    }
}