using System;
using System.IO;
using HuggingFace.API;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// A voice to text converter based on Hugging Face's API. <see https://huggingface.co/>
/// </summary>
public class VoiceCommandRecorder : Singleton<VoiceCommandRecorder>
{
    bool isRecording = false;
    private AudioClip clip;
    private byte[] bytes;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRecording)
        {
            StartRecording();
        }
        else if (!Input.GetKeyDown(KeyCode.Space) && isRecording)
        {
            StopRecording();
        }
    }

    private void StartRecording() {
        clip = Microphone.Start(null, false, 10, 44100);
        isRecording = true;
    }

    private void StopRecording() {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        isRecording = false;
        SendRecording();
    }

    private void SendRecording() {
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            Debug.Log($"Phrase recognised: {response}");
        }, error => {
            Debug.LogError(error);
        });
    }

    /// <summary>
    /// Sample code given from Hugging Face's API documentation.
    /// </summary>
    /// <param name="samples"></param>
    /// <param name="frequency"></param>
    /// <param name="channels"></param>
    /// <returns></returns>
    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}