public class RainbowOfHopeSpell : Spell
{
    public float multiplier;
    public float duration;
    
    public Fire fire;

    public override void Execute()
    {
        base.Execute();
        fire.SetInterval(fire.shootingInterval / multiplier);
        Invoke(nameof(Finish), duration);
    }
    
    void Finish()
    {
        fire.SetInterval(fire.shootingInterval * multiplier);
    }
}