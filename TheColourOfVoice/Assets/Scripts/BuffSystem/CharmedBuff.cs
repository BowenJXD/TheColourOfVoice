public class CharmedBuff : Buff
{
    private Attack attack;
    private Health health;
    private Painter painter;
    private Movement movement;
    private PaintColor cachedPaintColor;
    
    public override void OnApply(BuffOwner buffOwner)
    {
        base.OnApply(buffOwner);
        if (buffOwner.TryGetComponentInChildren(out Attack attack))
        {
            this.attack = attack;
            attack.gameObject.SetActive(false);
        }
        if (buffOwner.TryGetComponentInChildren(out Painter painter))
        {
            this.painter = painter;
            (cachedPaintColor, painter.paintColor) = (painter.paintColor, PaintColor.Magenta);
        }
        if (buffOwner.TryGetComponentInChildren(out Movement movement))
        {
            this.movement = movement;
            movement.speedModifiers[name] = 4f;
        }
    }
    
    public override void OnRemove()
    {
        base.OnRemove();
        if (attack) attack.gameObject.SetActive(true);
        if (painter) painter.paintColor = cachedPaintColor;
        if (movement) movement.speedModifiers.Remove(name);
    }
}