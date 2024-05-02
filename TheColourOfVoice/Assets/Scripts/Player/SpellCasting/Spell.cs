using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Spell : MonoBehaviour, ISetUp
{
    [Tooltip("The word to shout to trigger the spell.")]
    public Sprite spellImage;
    public string spellName;
    public float cooldown;
    [Multiline]public string spellDescription;
    [ShowInInspector] [ReadOnly] float remainingCD;
    public bool isInCD => remainingCD > 0;
    public bool needCasting;
    public float recoil;

    protected CastConfig currentConfig;
    public ParticleSystem ps;
    
    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        SpellManager.Instance.Register(this);
        if (!ps) ps = GetComponentInChildren<ParticleSystem>(true);
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        Init();
    }

    private void Update()
    {
        if (isInCD)
        {
            remainingCD -= Time.deltaTime;
            Lebug.Log(name + " CD", remainingCD);
        }
    }

    protected virtual void Init()
    {
        remainingCD = 0;
    }
    
    protected virtual void Unregister()
    {
        SpellManager.Instance.Unregister(this);
    }

    public virtual void StartCasting(CastConfig config)
    {
        if (isInCD)
        {
            Debug.LogWarning($"Spell is in cooldown. Remaining time: {remainingCD}");
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
        remainingCD = cooldown;
        
        if (ps)
        {
            ps.gameObject.SetActive(true);
            ps.Play();
        }
    }

    public virtual float GetCooldownTime()
    {
        return cooldown;
    }

    public virtual float GetRemainingCD()
    {
        return remainingCD;
    }
}