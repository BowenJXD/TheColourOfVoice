using System;
using UnityEngine;

public class GlowingConfidenceSpell : Spell
{
    public int duration = 5;
    public float moveMultiplier = 1.5f;

    public Movement movement;
    public Health health;
    public Painter painter;
    public LightAnim effect;

    LoopTask loopTask;

    public override void SetUp()
    {
        base.SetUp();
        if (!painter) painter = GetComponent<Painter>();
        if (!effect) effect = GetComponentInChildren<LightAnim>(true);
        loopTask = new LoopTask { interval = duration, finishAction = EndBuff };
    }

    public override void Execute()
    {
        base.Execute();
        
        loopTask.interval = duration * (1 + currentConfig.peakVolume);
        if (!loopTask.isPlaying)
        {
            StartBuff();
        }
        Debug.Log($"Execute {triggerWords} with duration {loopTask.interval}.");        
        loopTask.Start();
    }

    void StartBuff()
    {
        painter.enabled = true;
        if (movement) movement.Speed *= moveMultiplier;
        if (effect)
        {
            effect.gameObject.SetActive(true);
            new LoopTask{finishAction = effect.Execute, interval = Mathf.Max(loopTask.interval - effect.duration, 0)}.Start();
        }
        if (health) health.invincible = true;
    }

    void EndBuff()
    {
        painter.enabled = false;
        if (movement) movement.Speed /= moveMultiplier;
        if (effect) effect.gameObject.SetActive(false);
        if (health) health.invincible = false;
    }

    private void OnDisable()
    {
        if (loopTask.isPlaying)
        {
            loopTask.Stop();
            EndBuff();
        }
    }
}