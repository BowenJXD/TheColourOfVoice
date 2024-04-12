using System.Collections;

public class DeathCon : ConditionBehaviour
{
    private Health health;

    public override void SetUp()
    {
        base.SetUp();
        health = GetComponent<Health>();
    }
    
    protected override void Init()
    {
        health.OnDeath += () => StartCoroutine(Execute());
    }
    
    protected override void Deinit()
    {
        health.OnDeath -= () => StartCoroutine(Execute());
    }
}