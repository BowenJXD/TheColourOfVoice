using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAnim : MonoBehaviour, ISetUp
{
    public Light2D light2D;
    
    public float duration = 2f;
    public float intensity = 0.5f;
    public int intervalCount = 2;
    
    float initialIntensity;

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        light2D = GetComponent<Light2D>();
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }

    public void Execute()
    {
        Sequence sequence = DOTween.Sequence();
        initialIntensity = light2D.intensity;
        float interval = duration / (intervalCount * 2 + 1);
        for (int i = 0; i < intervalCount; i++)
        {
            sequence.Append(DOTween.To(() => light2D.intensity, x => light2D.intensity = x, intensity, interval));
            sequence.Append(DOTween.To(() => light2D.intensity, x => light2D.intensity = x, initialIntensity, interval));
        }
        sequence.Append(DOTween.To(() => light2D.intensity, x => light2D.intensity = x, intensity, interval));
        sequence.AppendCallback(() => light2D.intensity = initialIntensity);
        sequence.Play();
    }
}