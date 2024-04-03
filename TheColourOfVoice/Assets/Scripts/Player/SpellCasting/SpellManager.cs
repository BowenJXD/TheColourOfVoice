using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public struct CastConfig
{
    public float chantTime;
    public float peakVolume;
    public ConfidenceLevel confidenceLevel;
}

/// <summary>
///  Spell manager that manages all the spells in the game.
/// </summary>
public class SpellManager : Singleton<SpellManager>
{
    public Dictionary<string, Spell> spells = new();
    private Spell currentSpell;

    private CastState castState;
    
    enum CastState
    {
        Null,
        Chanting,
        Casting,
        ReleaseReady,
    }

    private void OnEnable()
    {
        StartChanting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && castState == CastState.ReleaseReady)
        {
            Release();
        }
    }

    void StartChanting()
    {
        VoiceInputSystem.Instance.SetActive(true);
        castState = CastState.Chanting;
        Debug.Log("Start chanting.");
    }

    void TryCast(PhraseRecognizedEventArgs recognizedEventArgs)
    {
        if (spells.TryGetValue(recognizedEventArgs.text, out var spell))
        {
            CastConfig config = new CastConfig
            {
                chantTime = (float)recognizedEventArgs.phraseDuration.TotalSeconds,
                peakVolume = 0, // TODO: Implement peak volume
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