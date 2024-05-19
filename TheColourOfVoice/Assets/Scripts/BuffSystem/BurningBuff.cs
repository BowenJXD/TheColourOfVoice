public class BurningBuff : Buff
{
    RageBehaviour rage;
    
    public override bool CanApply(BuffOwner buffOwner)
    {
        return buffOwner.TryGetComponent(out rage);
    }

    public override void OnApply(BuffOwner buffOwner)
    {
        base.OnApply(buffOwner);
        if (buffOwner.TryGetComponent(out rage))
        {
            rage.Ignite();
            rage.onExtinguish += Remove;
        }
    }
}