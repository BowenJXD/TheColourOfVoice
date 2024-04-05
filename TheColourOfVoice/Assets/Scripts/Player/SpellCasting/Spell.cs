using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Spell : MonoBehaviour
{
    [Tooltip("The word to shout to trigger the spell.")]
    public string spellName;

    public bool needCasting;

    protected CastConfig currentConfig;

    private void OnEnable()
    {
        Init();
    }

    protected virtual void Init()
    {
        SpellManager.Instance.Register(this);
    }
    
    private void OnDisable()
    {
        Deinit();
    }
    
    protected virtual void Deinit()
    {
        SpellManager.Instance.Unregister(this);
    }

    public virtual void StartCasting(CastConfig config)
    {
        currentConfig = config;
        if (needCasting)
        {
            Invoke(nameof(EndCasting), config.chantTime);
        }
        else
        {
            Execute();
        }
        Debug.Log($"Start casting {spellName}.");
    }

    /// <summary>
    /// Reset on trigger
    /// </summary>
    public Action onEndCasting;
    
    protected virtual void EndCasting()
    {
        onEndCasting?.Invoke();
        onEndCasting = null;
    }
    
    public virtual void Execute()
    {
        Debug.Log($"Execute {spellName}.");
    }
}