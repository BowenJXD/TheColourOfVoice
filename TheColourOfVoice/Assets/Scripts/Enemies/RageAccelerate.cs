using UnityEngine;

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
        movement.speed *= multiplier;
    }

    protected override void FinishRage()
    {
        base.FinishRage();
        movement.speed /= multiplier;
    }
}