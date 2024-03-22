using DG.Tweening;

/// <summary>
/// Delay a task for a certain amount of time and certain amount of loops.
/// Based on DOTween.
/// </summary>
public class DelayTask
{
    public float time;
    
    public int loop;
    
    public System.Action action;
    
    Sequence sequence;

    public void Start()
    {
        sequence = DOTween.Sequence();
        sequence.AppendInterval(time);
        sequence.OnStepComplete(Finish);
        sequence.SetLoops(loop);
        sequence.Play();
    }

    public void Pause()
    {
        if (sequence != null && sequence.IsPlaying())
        {
            sequence.Pause();
        }
    }

    public void Resume()
    {
        if (sequence != null && !sequence.IsPlaying())
        {
            sequence.Play();
        }
    }
    
    public void Stop()
    {
        if (sequence != null && sequence.IsPlaying())
        {
            sequence.Kill();
        }
    }

    public void Finish()
    {
        action();
    }
}