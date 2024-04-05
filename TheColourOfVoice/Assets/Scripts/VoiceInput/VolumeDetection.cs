using System.Linq;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
///  A singleton class that manages the microphone and allows retrieving the peak volume of the mic within a duration.
/// </summary>
public class VolumeDetection : Singleton<VolumeDetection>
{
    public int sampleRate = 44100;
    public int decibelScale = 2;
    AudioClip microphoneClip;

    void Start()
    {
        // Start recording audio from the default microphone
        microphoneClip = Microphone.Start(null, true, 3000, sampleRate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">In seconds</param>
    /// <returns></returns>
    public float GetPeakVolume(float duration, bool decibel = true)
    {
        // Calculate the number of samples to extract
        int sampleRate = microphoneClip.frequency; // Samples per second
        int totalSamples = (int)(duration * sampleRate);

        // Calculate the starting sample index
        int position = Microphone.GetPosition(null);
        int startSample = position - totalSamples;
        if (startSample < 0)
        {
            startSample = 0;
            totalSamples = position;
        }

        // Create an array to hold the samples
        float[] samples = new float[totalSamples * microphoneClip.channels];
        if (samples.Length == 0) return 0;

        // Extract the samples from the clip
        microphoneClip.GetData(samples, startSample);

        float peakVolume = samples.Max();
        
        if (decibel && peakVolume > 0 && decibelScale > 1)
        {
            float decibelVolume = Mathf.Log10(Mathf.Pow(10, decibelScale) * peakVolume + 1) / decibelScale;
            decibelVolume = Mathf.Clamp(decibelVolume, 0, 1);
            return decibelVolume;
        }

        return peakVolume;
    }

    protected override void OnDestroy()
    {
        // Stop the microphone when the script is destroyed
        Microphone.End(null);
    }
}