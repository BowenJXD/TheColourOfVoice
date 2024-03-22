using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public struct CastConfig
{
    public float castTime;
    public float peakVolume;
    public ConfidenceLevel confidenceLevel;
}

public class SpellManager : Singleton<SpellManager>
{
    public Dictionary<string, Spell> spells = new();
    private Spell currentSpell;

    public int timeTolerance = 10;

    private CastState castState;
    
    AudioClip clip;
    
    DelayTask chantTask;

    enum CastState
    {
        Null,
        Chanting,
        Casting,
        ReleaseReady,
    }

    protected override void Awake()
    {
        base.Awake();
        chantTask = new DelayTask()
        {
            time = timeTolerance,
            action = () =>
            {
                Microphone.End(null);
                castState = CastState.Null;
                Debug.LogWarning("Chanting time out.");
            },
            loop = 1,
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && castState == CastState.Null)
        {
            StartChanting();
        }
        if (Input.GetKeyDown(KeyCode.Space) && castState == CastState.ReleaseReady)
        {
            Release();
        }
    }

    void StartChanting()
    {
        clip = Microphone.Start(null, false, timeTolerance, 44100);
        castState = CastState.Chanting;

        chantTask.Start();
    }

    void TryCast(PhraseRecognizedEventArgs recognizedEventArgs)
    {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        var volume = samples.Max();
        
        chantTask.Stop();
        
        if (spells.TryGetValue(recognizedEventArgs.text, out var spell))
        {
            CastConfig config = new CastConfig
            {
                castTime = (float)recognizedEventArgs.phraseDuration.TotalSeconds,
                peakVolume = volume,
                confidenceLevel = recognizedEventArgs.confidence
            };
            spell.StartCasting(config);
            spell.onEndCasting += () => castState = CastState.ReleaseReady;
            
            currentSpell = spell;
            castState = spell.needCasting ? CastState.Casting : CastState.Null;
        }
        else
        {
            Debug.LogWarning($"Spell {recognizedEventArgs.text} not found.");
        }
    }

    void Release()
    {
        if (!currentSpell)
        {
            Debug.LogWarning("No spell to release.");
            return;
        }
        currentSpell.Execute();
        currentSpell = null;
        castState = CastState.Null;
    }
    
    public void Register(Spell spell)
    {
        if (spells.ContainsKey(spell.spellName))
        {
            Debug.LogWarning($"Spell with name {spell.spellName} already exists.");
            return;
        }
        spells.Add(spell.spellName, spell);
        VoiceInputSystem.Instance.Register(spell.spellName, TryCast);
    }
    
    public void Unregister(Spell spell)
    {
        if (spells.ContainsKey(spell.spellName))
        {
            spells.Remove(spell.spellName);
            VoiceInputSystem.Instance.Unregister(spell.spellName);
        }
    }
}