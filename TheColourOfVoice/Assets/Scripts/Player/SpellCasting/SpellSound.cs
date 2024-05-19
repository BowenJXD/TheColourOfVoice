using System;
using UnityEngine;

public enum FMODCastState
{
    None,
    Casting,
    ReleaseReady,
    Released,
}

public class SpellSound : MonoBehaviour, ISetUp
{
    FMODUnity.StudioEventEmitter emitter;
    private Spell spell;
    
    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        spell = GetComponent<Spell>();
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        SpellManager.Instance.onCastStateChange += OnCastStateChange;
        SpellManager.Instance.onRelease += OnRelease;
    }

    private void OnCastStateChange(CastState state)
    {
        if (SpellManager.Instance.currentSpell == spell)
        {
            FMODCastState fmodState = FMODCastState.None;
            switch (state)
            {
                case CastState.Casting:
                    fmodState = FMODCastState.Casting;
                    break;
                case CastState.ReleaseReady:
                    fmodState = FMODCastState.ReleaseReady;
                    break;
                default:
                    fmodState = FMODCastState.None;
                    break;
            }
            emitter.SetParameter("CastState", (int)fmodState);
            // emitter.Play();
        }
    }

    private void OnRelease()
    {
        if (SpellManager.Instance.currentSpell == spell)
        {
            emitter.SetParameter("CastState", (int)FMODCastState.Released);
            // emitter.Play();
        }
    }
}