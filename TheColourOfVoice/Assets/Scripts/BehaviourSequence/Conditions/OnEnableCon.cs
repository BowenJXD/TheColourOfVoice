public class OnEnableCon : ConditionBehaviour
{
    protected override void Init()
    {
        base.Init();
        StartCoroutine(Execute());
    }
}