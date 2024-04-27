public class RageAccelerate : RageBehaviour
{
    public float multiplier = 2f;
    
    Movement movement;
    
    public override void SetUp()
    {
        base.SetUp();
        movement = GetComponent<Movement>();
    }

    protected override void StartRage()
    {
        base.StartRage();
        movement.Speed *= multiplier;
    }

    protected override void FinishRage()
    {
        base.FinishRage();
        movement.Speed /= multiplier;
    }
}