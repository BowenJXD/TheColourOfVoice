using System;
using System.Linq;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class VolumeControlOrthoSize : Singleton<VolumeControlOrthoSize>
{
    public CinemachineVirtualCamera virtualCamera;
    float originalSize = 0;
    float targetSize = 0;
    public float decreaseSpeed = 0.2f;
    public float increaseSpeed = 0.2f;
    [Range(0,1)][Tooltip("Volume Threshold is an assumption of max volume that the microphone can receive. " +
                         "It is used to scale the orthographic size to make it to suit different environment.")]
    public float initialVolumeThreshold = 1f;
    [ShowInInspector] [ReadOnly] float volumeThreshold = 0f;
    [Tooltip("The maximum scale of the changeable orthographic size compared to the original size.")]
    public float maxSizeScale = 2f;

    void Start()
    {
        if (!virtualCamera) virtualCamera = GetComponent<CinemachineVirtualCamera>();
        originalSize = virtualCamera.m_Lens.OrthographicSize;
        volumeThreshold = initialVolumeThreshold;
    }

    void Update()
    {
        // Calculate the volume of the audio received by the microphone
        float volumeFactor = GetVolumeFactor();
        float currentSize = virtualCamera.m_Lens.OrthographicSize;
        float inputSize = originalSize + maxSizeScale * originalSize * volumeFactor;
        if (inputSize > targetSize) targetSize = inputSize;
        float delta; 
        if (currentSize < targetSize)
        {
            delta = increaseSpeed;
        }
        else
        {
            targetSize = inputSize;
            delta = -decreaseSpeed;
        }

        delta *= maxSizeScale * Time.deltaTime + Mathf.Abs(currentSize - targetSize);

        virtualCamera.m_Lens.OrthographicSize += delta;
    }

    float GetVolumeFactor()
    {
        float volume = VolumeDetection.Instance.GetPeakVolume(Time.deltaTime);
        float result = volume / volumeThreshold;
        if (result > 1)
        {
            volumeThreshold = volume;
            result = 1;
        }
        return result;
    }

    private void OnDestroy()
    {
        if (virtualCamera) virtualCamera.m_Lens.OrthographicSize = originalSize;
    }
}