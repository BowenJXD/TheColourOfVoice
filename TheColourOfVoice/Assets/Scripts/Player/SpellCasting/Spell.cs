using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Spell : MonoBehaviour, ISetUp
{
    [Tooltip("The word to shout to trigger the spell.")]
    public string triggerWords;
    public float cooldown;
    [ShowInInspector] [ReadOnly] protected float remainingCD;
    
    [ReadOnly] public int spellIndex;
    public string spellName;
    public string spellDescription;
    public Sprite spellImage;
    
    public bool isInCD => remainingCD > 0;
    public bool needCasting;
    public float recoil;

    protected CastConfig currentConfig;
    public ParticleSystem ps;

    public bool upgraded;
    
    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        if (!ps) ps = GetComponentInChildren<ParticleSystem>(true);
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        SpellManager.Instance.Register(this);
        Init();
    }

    private void Update()
    {
        if (isInCD)
        {
            remainingCD -= Time.deltaTime;
            Lebug.Log(name + " CD", remainingCD);
            if (remainingCD <= 0)
            {
                remainingCD = 0;
                EndCD();
            }
        }
    }

    private void EndCD()
    {
        SpellUIManager.Instance.OnSkillCDComplete(this);
    }

    protected virtual void Init()
    {
        remainingCD = 0;
        Lebug.Log(name + " CD", remainingCD);
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
        Invoke(nameof(EndCasting), config.chantTime);
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
        Debug.Log($"Execute {triggerWords}.");
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

    private void OnDisable()
    {
        SpellManager.Instance.Unregister(this);
    }

    public virtual void Upgrade()
    {
        upgraded = true;
        cooldown = 0.1f;
        remainingCD = Mathf.Min(remainingCD, cooldown);
        // throw new NotImplementedException();
        Debug.Log(name + " upgraded!");
    }
}