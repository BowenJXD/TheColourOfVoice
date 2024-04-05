using UnityEngine;

public class NimblePenSpell : Spell, ISetUp
{
    public int duration = 5;
    public float moveMultiplier = 1.5f;

    public Movement movement;
    public Painter painter;
    public GameObject effect;

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
        if (!effect) effect = transform.GetChild(0).gameObject;
        loopTask = new LoopTask { interval = duration, loop = 1, loopAction = EndBuff };
    }

    public override void Execute()
    {
        base.Execute();
        
        if (!loopTask.isPlaying)
        {
            StartBuff();
        }
        loopTask.interval = duration * (1 + currentConfig.peakVolume);
        Debug.Log($"Execute {spellName} with duration {loopTask.interval}.");        
        loopTask.Start();
    }

    void StartBuff()
    {
        painter.enabled = true;
        if (movement) movement.speed *= moveMultiplier;
        if (effect) effect.SetActive(true);
    }

    void EndBuff()
    {
        painter.enabled = false;
        if (movement) movement.speed /= moveMultiplier;
        if (effect) effect.SetActive(false);
    }

}