using DG.Tweening;
using UnityEngine;

namespace CodeTao
{
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

        protected override void SetUpTween()
        {
            base.SetUpTween();
            switch (shakeType)
            {
                case ShakeType.Position:
                    tween = transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
                    break;
                case ShakeType.Rotation:
                    tween = transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);
                    break;
                case ShakeType.Scale:
                    tween = transform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut);
                    break;
            }
        }
    }
}