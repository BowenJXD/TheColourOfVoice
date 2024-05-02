using System;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;

/// <summary>
/// A voice input system that uses the Windows Speech API to recognize keywords.
/// </summary>
public class VoiceInputSystem : Singleton<VoiceInputSystem>
{
    private Dictionary<string, KeywordRecognizer> keywordRecognizers = new();

    protected override void Awake()
    {
        base.Awake();
        /*new LoopTask{loop = -1, loopAction = () =>
        {
            SetActive(false);
            SetActive(true);
        }, interval = 10}.Start();*/
    }

    private void Start()
    {
        SetActive(true);
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            foreach (var recognizer in keywordRecognizers.Values)
            {
                recognizer.Start();
            }
        }
        else
        {
            foreach (var recognizer in keywordRecognizers.Values)
            {
                recognizer.Stop();
            }
        }
    }
    
    public void Register(string key, Action<PhraseRecognizedEventArgs> action)
    {
        var recognizer = new KeywordRecognizer(new string[] { key });
        recognizer.OnPhraseRecognized += (e) => action(e);
        keywordRecognizers.Add(key, recognizer);
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