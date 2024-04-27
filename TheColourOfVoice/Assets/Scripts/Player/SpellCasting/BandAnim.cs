using System;
using DG.Tweening;
using UnityEngine;

public class BandAnim : MonoBehaviour
{
    private Vector3 originalPosition;
    public float recoilDisplacement = 1f;
    [Range(0,1)][Tooltip("The percentage of the cooldown time to recoil. The rest of the time will be used to return to the original position.")]
    public float recoilDuration = 0.2f;
    public Ease recoilEase = Ease.OutBack;
    
    public float shakeStrength = 1f;
    
    SpellManager spellManager;
    private Sequence shakeSequence;
    
    private void OnEnable()
    {
        spellManager = SpellManager.Instance;
        spellManager.OnCastStateChange += OnCastStateChange;
        originalPosition = transform.localPosition;
    }
    
    void OnCastStateChange(CastState state)
    {
        switch (state)
        {
            case CastState.Null:
                Sequence sequence = DOTween.Sequence();
                sequence.Append(transform.DOLocalMoveX(originalPosition.x - recoilDisplacement, recoilDuration).SetEase(recoilEase));
                sequence.Append(transform.DOLocalMoveX(originalPosition.x, spellManager.cooldownTime - recoilDuration).SetEase(recoilEase));
                sequence.Play();
                break;
            case CastState.Chanting:
                break;
            case CastState.Casting:
                shakeSequence = DOTween.Sequence();
                shakeSequence.Append(transform.DOShakePosition(1, shakeStrength));
                shakeSequence.SetLoops(-1);
                shakeSequence.Play();
                break;
            case CastState.ReleaseReady:
                if (shakeSequence != null && shakeSequence.IsPlaying())
                {
                    shakeSequence.Kill();
                    transform.localPosition = originalPosition;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnDisable()
    {
        shakeSequence?.Kill();
    }
}