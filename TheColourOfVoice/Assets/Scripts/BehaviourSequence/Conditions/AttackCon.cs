public class AttackCon : ConditionBehaviour
{
    private Attack attack;

    public override void SetUp()
    {
        base.SetUp();
        attack = GetComponentInChildren<Attack>(true);
    }
    
    protected override void Init()
    {
        attack.OnDamage += OnDamage;
    }

    private void OnDamage(Health health)
    {
        blackboard.Set(BBKey.TARGET, health.transform);
        StartCoroutine(Execute());
    }

    protected override void Deinit()
    {
        attack.OnDamage -= OnDamage;
    }
}