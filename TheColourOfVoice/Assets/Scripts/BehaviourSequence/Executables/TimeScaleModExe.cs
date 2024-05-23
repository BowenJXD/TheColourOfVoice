using DG.Tweening;
using UnityEngine;

public class TimeScaleModExe : ExecutableBehaviour
{
    public float timeScaleOverride = 0.2f;
    public float duration = 1;

    public static Tween tween;

    public override void SetUp()
    {
        base.SetUp();
        if (tween == null)
        {
            tween = TweenTimeScale(timeScaleOverride, duration);
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        tween.Restart();
    }

    Tween TweenTimeScale(float targetTimeScale, float duration)
    {
        // Store the current timeScale
        float initialTimeScale = Time.timeScale;

        // Create the tween
        Tween result = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, targetTimeScale, duration / 2).OnComplete(() =>
        {
            // Tween back to the initial timeScale if needed
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, initialTimeScale, duration / 2);
        });
        result.SetAutoKill(false).Pause();
        return result;
    }
}