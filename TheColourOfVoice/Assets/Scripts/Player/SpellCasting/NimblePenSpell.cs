public class NimblePenSpell : Spell, ISetUp
{
    public int duration = 5;
    public float moveMultiplier = 1.5f;

    public Movement movement;
    public Painter painter;
    
    LoopTask loopTask;

    protected override void Init()
    {
        base.Init();
        if (!IsSet) SetUp();
    }

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        if (!painter) painter = GetComponent<Painter>();
        loopTask = new LoopTask { interval = duration, loop = 1, loopAction = EndBuff };
    }

    public override void Execute()
    {
        base.Execute();
        
        if (!loopTask.isPlaying)
        {
            painter.enabled = true;
            if (movement) movement.speed *= moveMultiplier;
        }
        loopTask.Start();
    }

    void EndBuff()
    {
        painter.enabled = false;
        if (movement) movement.speed /= moveMultiplier;
    }

}