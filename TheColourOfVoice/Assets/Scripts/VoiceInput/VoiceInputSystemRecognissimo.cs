using System;
using System.Collections.Generic;
using Recognissimo.Components;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// A voice input system that uses the Windows Speech API to recognize keywords.
/// </summary>
public class VoiceInputSystemRecognissimo : Singleton<VoiceInputSystemRecognissimo>
{
    public VoiceControl engine;

    private void Start()
    {
        engine = GetComponent<VoiceControl>();
        engine.AsapMode = true;
        engine.StartProcessing();
    }

    public void Register(string key, Action<string> action)
    {
        engine.Commands.Add(new VoiceControlCommand(key, () => action(key)));
    }
    
    public void Unregister(string key)
    {
        engine.Commands.RemoveAll(c => c.phrase == key);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        engine.Commands.Clear();
        engine.StopProcessing();
    }
}