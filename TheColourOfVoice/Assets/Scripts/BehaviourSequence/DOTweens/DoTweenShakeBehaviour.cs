using DG.Tweening;
using UnityEngine;

/// <summary>
///  震动类型
/// </summary>
public enum ShakeType
{
    Position,
    Rotation,
    Scale
}
    
/// <summary>
///  DoTween震动节点
/// </summary>
public class DoTweenShakeBehaviour : DoTweenBehaviour
{
    public ShakeType shakeType = ShakeType.Position;
    public float strength = 1;
    public int vibrato = 10;
    public float randomness = 90;
    public bool snapping = false;
    public bool fadeOut = false;
    public bool reset = false;

    protected override void SetUpTween()
    {
        base.SetUpTween();
        switch (shakeType)
        {
            case ShakeType.Position:
                tween.Append(target.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut));
                break;
            case ShakeType.Rotation:
                tween.Append(target.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut));
                break;
            case ShakeType.Scale:
                tween.Append(target.DOShakeScale(duration, strength, vibrato, randomness, fadeOut));
                break;
        }
        if (reset) tween.OnComplete(ResetFromTween).OnKill(ResetFromTween);
    }

    void ResetFromTween()
    {
        switch (shakeType)
        {
            case ShakeType.Position:
                target.localPosition = Vector3.zero;
                break;
            case ShakeType.Rotation:
                target.localRotation = Quaternion.identity;
                break;
            case ShakeType.Scale:
                target.localScale = Vector3.one;
                break;
        }
    }
}