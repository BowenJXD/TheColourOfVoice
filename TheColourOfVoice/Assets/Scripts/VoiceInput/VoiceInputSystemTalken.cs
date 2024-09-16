/*using System;
using System.Collections.Generic;
using LeastSquares.Talken;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// A voice input system that uses the Windows Speech API to recognize keywords.
/// </summary>
public class VoiceInputSystemTalken : Singleton<VoiceInputSystemTalken>
{
    public VoiceCommandEngine engine;

    private void Start()
    {
        engine = GetComponent<VoiceCommandEngine>();
        engine.StartListening();
    }

    public void Register(string key, Action<string> action)
    {
        engine.AddCommand(key, action);
    }
    
    public void Unregister(string key)
    {
        engine.RemoveCommand(key);
    }
}*/