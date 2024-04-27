using Sirenix.OdinInspector;

public class SpeedBuff : Buff
{
    [MinValue(0.000001)]
    public float multiplier = 2f;
    
    private Movement movement;

    public override void OnApply(BuffOwner buffOwner)
    {
        base.OnApply(buffOwner);
        if (buffOwner.TryGetComponent(out Movement movement))
        {
            this.movement = movement;
            movement.Speed *= multiplier;
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (movement) movement.Speed /= multiplier;
    }
}