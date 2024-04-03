using System;
using DG.Tweening;
using UnityEngine;

public class MoveAnim : MonoBehaviour
    {
        //
        public float period = 0.5f;
        public float rotationDelta = 30;
        public float scaleYDelta = 0.2f;
        public float scaleXDelta = 0.2f;
        
        public Ease rotationEase = Ease.InOutQuad;
        public Ease scaleEase = Ease.InOutQuad;
        
        public float minMoveSpeed = 0.5f;
        public float maxMoveSpeed = 1.5f;

        private Sequence rotationSequence;
        private Sequence scaleSequence;
        
        Vector3 startRotation;
        Vector3 startScale;

        private void OnEnable()
        {
            StartAnim();
        }

        private void OnDisable()
        {
            EndAnim(true);
        }

        public void StartAnim()
        {
            if (rotationSequence == null)
            {
                rotationSequence = DOTween.Sequence();
                rotationSequence.Kill();
                startRotation = transform.eulerAngles;
            }
            if (!rotationSequence.IsActive())
            {
                Vector3 rotation1 = new Vector3(startRotation.x, startRotation.y, startRotation.z + rotationDelta);
                Vector3 rotation2 = new Vector3(startRotation.x, startRotation.y, startRotation.z - rotationDelta);
                transform.DORotate(rotation2, period / 2).OnComplete(
                    () =>
                    {
                        rotationSequence = DOTween.Sequence();
                        rotationSequence.Append(transform.DORotate(rotation1, period).SetEase(rotationEase));
                        rotationSequence.Append(transform.DORotate(rotation2, period).SetEase(rotationEase));
                        rotationSequence.SetLoops(-1);
                    });
            }
            if (scaleSequence == null)
            {
                scaleSequence = DOTween.Sequence();
                scaleSequence.Kill();
                startScale = transform.localScale;
            }
            if (!scaleSequence.IsActive())
            {
                startScale = transform.localScale;
                Vector3 scale1 = new Vector3(startScale.x * (1 - scaleXDelta), startScale.y * (1 + scaleYDelta), 1);
                Vector3 scale2 = new Vector3(startScale.x * (1 + scaleXDelta), startScale.y * (1 - scaleYDelta), 1);
                transform.DOScale(scale2, period / 2).OnComplete(
                    () =>
                    {
                        scaleSequence = DOTween.Sequence();
                        scaleSequence.Append(transform.DOScale(scale1, period / 2).SetEase(scaleEase));
                        scaleSequence.Append(transform.DOScale(scale2, period / 2).SetEase(scaleEase));
                        scaleSequence.SetLoops(-1);
                    });
            }
        }

        public void EndAnim(bool immediate = false)
        {
            if (rotationSequence != null)
            {
                rotationSequence?.Kill();
                if (immediate)
                {
                    transform.eulerAngles = startRotation;
                }
                else
                {
                    transform.DORotate(startRotation, period / 2).SetEase(rotationEase);
                }
            }
            
            if (scaleSequence != null)
            {
                scaleSequence?.Kill();
                if (immediate)
                {
                    transform.localScale = startScale;
                }
                else
                {
                    transform.DOScale(startScale, period / 2).SetEase(scaleEase);
                }
            }
        }
    }