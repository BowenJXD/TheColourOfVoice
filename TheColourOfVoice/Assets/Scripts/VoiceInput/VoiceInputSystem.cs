using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// A voice input system that uses the Windows Speech API to recognize keywords.
/// </summary>
public class VoiceInputSystem : Singleton<VoiceInputSystem>
{
    private Dictionary<string, KeywordRecognizer> keywordRecognizers = new();

    private bool isActive;
    
    protected override void Awake()
    {
        base.Awake();
        /*new LoopTask{loop = -1, loopAction = () =>
        {
            SetActive(false);
            SetActive(true);
        }, interval = 10}.Start();*/
    }

    private void OnEnable()
    {
        SetActive(true);
    }
    
    private void OnDisable()
    {
        SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetActive(false);
            SetActive(true);
        }
    }

    public void SetActive(bool active)
    {
        string message = "Recognizers: ";
        if (active)
        {
            foreach (var kvp in keywordRecognizers)
            {
                kvp.Value.Start();
                message += $"{kvp.Key}, ";
            }
            message += "started.";
        }
        else
        {
            foreach (var kvp in keywordRecognizers)
            {
                kvp.Value.Stop();
                message += $"{kvp.Key}, ";
            }
            message += "stopped.";
        }
        Debug.Log(message);
        isActive = active;
    }
    
    public void Register(string key, Action<PhraseRecognizedEventArgs> action)
    {
        var recognizer = new KeywordRecognizer(new string[] { key });
        recognizer.OnPhraseRecognized += (e) => action(e);
        keywordRecognizers.Add(key, recognizer);
        recognizer.Start();
    }
    
    public void Unregister(string key)
    {
        if (keywordRecognizers.TryGetValue(key, out var recognizer))
        {
            recognizer.Stop();
            recognizer.Dispose();
            keywordRecognizers.Remove(key);
        }
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach (var recognizer in keywordRecognizers.Values)
        {
            recognizer.Stop();
            recognizer.Dispose();
        }
    }
}