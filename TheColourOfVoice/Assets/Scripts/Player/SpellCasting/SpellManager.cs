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
    public Dictionary<string, Spell> learntSpells = new();
    public float cooldownTime = 0.5f;

    public Fire defaultSpell;
    public Rigidbody2D rb;
    [ReadOnly] public Spell currentSpell;
    public List<Spell> allSpells;

    private CastState _castState;

    public CastState CastState
    {
        get => _castState;
        set{
            onCastStateChange?.Invoke(value);
            _castState = value;
        }
    }

    /// <summary>
    /// Called before trigger, reset on disable
    /// </summary>
    public Action<CastState> onCastStateChange;
    
    LoopTask coolDownTask;

    private void OnEnable()
    {
        LearnSpell(PlayerPrefs.GetInt("levelIndex", 0));
        StartChanting();
    }

    private void OnDisable()
    {
        currentSpell = null;
        onCastStateChange = null;
        onRelease = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CastState == CastState.ReleaseReady)
        {
            Release();
        }
    }

    void LearnSpell(int spellIndex)
    {
        spellIndex--;
        if (allSpells.Count > spellIndex && !learntSpells.ContainsValue(allSpells[spellIndex]))
        {
            allSpells[spellIndex].gameObject.SetActive(true);
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
        
        if (!learntSpells.TryGetValue(recognizedEventArgs.text, out var spell))
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
        Debug.Log($"Start Casting Spell: {spell.spellName}, " +
                  $"Chant Time: {config.chantTime}, " +
                  $"Peak Volume: {config.peakVolume}, " +
                  $"Confidence Level: {config.confidenceLevel}");

        currentSpell = spell;
        if (spell.needCasting)
        {
            spell.StartCasting(config);
            spell.onEndCasting += () => CastState = CastState.ReleaseReady;
            CastState = CastState.Casting;
            defaultSpell.enabled = false;
        }
        else
        {
            Release();
            CastState = CastState.Null;
            coolDownTask.Start();
        }
    }

    /// <summary>
    /// Reset on disable
    /// </summary>
    public Action onRelease;
    
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
        CastState = CastState.Null;
        currentSpell.Execute();
        onRelease?.Invoke();
        Vector2 direction = transform.right;
        if (rb) rb.AddForce(-direction * currentSpell.recoil, ForceMode2D.Impulse);
        currentSpell = null;
        
        coolDownTask.Start();
    }

    public bool Register(Spell spell)
    {
        if (learntSpells.ContainsKey(spell.spellName))
        {
            Debug.LogWarning($"Spell with name {spell.spellName} already exists.");
            return false;
        }
        learntSpells.Add(spell.spellName, spell);
        VoiceInputSystem.Instance.Register(spell.spellName, TryCast);
        return true;
    }
    
    public void Unregister(Spell spell)
    {
        if (learntSpells.ContainsKey(spell.spellName))
        {
            learntSpells.Remove(spell.spellName);
            VoiceInputSystem.Instance.Unregister(spell.spellName);
        }
    }

    public bool ChangeName(string oldName, string newName)
    {
        if (learntSpells.ContainsKey(oldName))
        {
            if (learntSpells.ContainsKey(newName))
            {
                Debug.LogWarning($"Spell with name {newName} already exists.");
                return false;
            }
            Spell spell = learntSpells[oldName];
            learntSpells.Remove(oldName);
            learntSpells.Add(newName, spell);
            VoiceInputSystem.Instance.Unregister(oldName);
            VoiceInputSystem.Instance.Register(newName, TryCast);
            Debug.Log($"Spell with name {oldName} changed to {newName}.");
            return true;
        }
        Debug.LogWarning($"Spell with name {oldName} not found.");
        return false;
    }
}