using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Windows.Speech;

public struct CastConfig
{
    public float chantTime;
    public float peakVolume;
    public ConfidenceLevel confidenceLevel;
}

public enum CastState
{
    /// <summary>
    /// Does not receive any voice input.
    /// To chanting state after cooldown time.
    /// </summary>
    Null,
    /// <summary>
    /// Awaits for voice input.
    /// To casting state when a spell that needs casting is recognized.
    /// </summary>
    Chanting,
    /// <summary>
    /// Awaits for the spell to prepare release.
    /// Can only cast one spell at a time.
    /// To release state when the player releases the spell.
    /// </summary>
    Casting,
    /// <summary>
    /// Awaits for the player to release the spell.
    /// To null state when the spell is released.
    /// </summary>
    ReleaseReady,
}

/// <summary>
///  Spell manager that manages all the spells in the game.
/// </summary>
public class SpellManager : Singleton<SpellManager>
{
    public Dictionary<string, Spell> spells = new();
    public float cooldownTime = 0.5f;

    public Fire defaultSpell;
    public Rigidbody2D rb;
    [ReadOnly]public Spell currentSpell;

    private CastState _castState;

    public CastState CastState
    {
        get => _castState;
        set{
            _castState = value;
            OnCastStateChange?.Invoke(_castState);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Action<CastState> OnCastStateChange;
    
    LoopTask coolDownTask;

    private void OnEnable()
    {
        StartChanting();
    }

    private void OnDisable()
    {
        VoiceInputSystem.Instance.SetActive(false);
        currentSpell = null;
        OnCastStateChange = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CastState == CastState.ReleaseReady)
        {
            Release();
        }
    }

    void StartChanting()
    {
        VoiceInputSystem.Instance.SetActive(true);
        CastState = CastState.Chanting;
        coolDownTask = new LoopTask{ finishAction = EndCD, interval = cooldownTime};
    }
    
    void EndCD()
    {
        CastState = CastState.Chanting;
        defaultSpell.enabled = true;
    }

    void TryCast(PhraseRecognizedEventArgs recognizedEventArgs)
    {
        if (CastState == CastState.Null)
        {
            Debug.LogWarning("In Cooldown.");
            return;
        }
        
        if (!spells.TryGetValue(recognizedEventArgs.text, out var spell))
        {
            Debug.LogWarning($"Spell {recognizedEventArgs.text} not found.");
            return;
        }

        if (currentSpell != null && spell.needCasting)
        {
            Debug.LogWarning("Another spell is casting.");
            return;
        }

        if (spell.isInCD)
        {
            Debug.LogWarning($"Spell {recognizedEventArgs.text} is in cooldown.");
            return;
        }
            
        float duration = (float)recognizedEventArgs.phraseDuration.TotalSeconds;
        float peakVolume = VolumeDetection.Instance.GetPeakVolume(duration);
            
        CastConfig config = new CastConfig
        {
            chantTime = (float)recognizedEventArgs.phraseDuration.TotalSeconds,
            peakVolume = peakVolume,
            confidenceLevel = recognizedEventArgs.confidence
        };
        spell.StartCasting(config);
        spell.onEndCasting += () => CastState = CastState.ReleaseReady;
        Debug.Log($"Start Casting Spell: {spell.spellName}, " +
                  $"Chant Time: {config.chantTime}, " +
                  $"Peak Volume: {config.peakVolume}, " +
                  $"Confidence Level: {config.confidenceLevel}");

        if (spell.needCasting)
        {
            CastState = CastState.Casting;
            currentSpell = spell;
            defaultSpell.enabled = false;
        }
        else
        {
            CastState = CastState.Null;
            coolDownTask.Start();
        }
    }

    /// <summary>
    /// Only for spells that need casting.
    /// </summary>
    void Release()
    {
        if (!currentSpell)
        {
            Debug.LogWarning("No spell to release.");
            return;
        }
        currentSpell.Execute();
        Vector2 direction = transform.right;
        if (rb) rb.AddForce(-direction * currentSpell.recoil, ForceMode2D.Impulse);
        currentSpell = null;
        
        CastState = CastState.Null;
        coolDownTask.Start();
    }

    public bool Register(Spell spell)
    {
        if (spells.ContainsKey(spell.spellName))
        {
            Debug.LogWarning($"Spell with name {spell.spellName} already exists.");
            return false;
        }
        spells.Add(spell.spellName, spell);
        VoiceInputSystem.Instance.Register(spell.spellName, TryCast);
        return true;
    }
    
    public void Unregister(Spell spell)
    {
        if (spells.ContainsKey(spell.spellName))
        {
            spells.Remove(spell.spellName);
            VoiceInputSystem.Instance.Unregister(spell.spellName);
        }
    }

    public bool ChangeName(string oldName, string newName)
    {
        if (spells.ContainsKey(oldName))
        {
            if (spells.ContainsKey(newName))
            {
                Debug.LogWarning($"Spell with name {newName} already exists.");
                return false;
            }
            Spell spell = spells[oldName];
            spells.Remove(oldName);
            spells.Add(newName, spell);
            VoiceInputSystem.Instance.Unregister(oldName);
            VoiceInputSystem.Instance.Register(newName, TryCast);
            Debug.Log($"Spell with name {oldName} changed to {newName}.");
            return true;
        }
        Debug.LogWarning($"Spell with name {oldName} not found.");
        return false;
    }
}