using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Spell : MonoBehaviour, ISetUp
{
    [Tooltip("The word to shout to trigger the spell.")]
    public string spellName;
    public float cooldown;
    [ReadOnly] public bool isInCD; 
    public bool needCasting;
    public float recoil;

    protected CastConfig currentConfig;

    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        SpellManager.Instance.Register(this);
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        Init();
    }

    protected virtual void Init()
    {
        isInCD = false;
    }
    
    protected virtual void Unregister()
    {
        SpellManager.Instance.Unregister(this);
    }

    public virtual void StartCasting(CastConfig config)
    {
        if (isInCD)
        {
            Debug.LogWarning("Spell is in cooldown.");
            return;
        }
        currentConfig = config;
        if (needCasting)
        {
            Invoke(nameof(EndCasting), config.chantTime);
        }
        else
        {
            Execute();
        }
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
        isInCD = true;
        Invoke(nameof(EndCD), cooldown);
    }
    
    void EndCD()
    {
        isInCD = false;
    }
}