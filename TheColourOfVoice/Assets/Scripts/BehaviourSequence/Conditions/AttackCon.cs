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
        attack.OnDamage += _ => StartCoroutine(Execute());
    }
    
    protected override void Deinit()
    {
        attack.OnDamage -= _ => StartCoroutine(Execute());
    }
}