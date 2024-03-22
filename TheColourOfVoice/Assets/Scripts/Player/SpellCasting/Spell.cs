using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Spell : MonoBehaviour
{
    [Tooltip("The word to shout to trigger the spell.")]
    public string spellName;

    public bool needCasting;

    private void OnEnable()
    {
        SpellManager.Instance.Register(this);
    }
    
    private void OnDisable()
    {
        SpellManager.Instance.Unregister(this);
    }

    public virtual void StartCasting(CastConfig config)
    {
        Invoke(nameof(EndCasting), config.chantTime);
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