using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Faint system for game objects, will freeze the game object for a certain amount of time when dead and reset <see cref="Health"/>
/// </summary>
public class Faint : MonoBehaviour, ISetUp
{
    public float faintDuration;
    protected float faintTimer;
    public float QTEEfficiency;

    public List<GameObject> inactivatingGameObjects;
    public List<Behaviour> disablingComponents;
    private Health health;
    private Collider2D col;
    private LayerMask inCache;
    private LayerMask exCache;
    public Animator ani;

    public Slider slider;

    private void OnEnable()
    {
        if (!IsSet) SetUp();
        health.OnDeath += StartFaint;
    }

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        health = GetComponent<Health>();
        col = GetComponent<Collider2D>();
    }
    
    public UnityEvent onFaint;
    public UnityEvent onEndFaint;

    private void Update()
    {
        if (faintTimer > 0)
        {
            float decrementMultiplier = 1;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                decrementMultiplier = QTEEfficiency;
            }
            faintTimer -= Time.deltaTime * decrementMultiplier;
            slider.value = faintTimer / faintDuration;
            if (faintTimer <= 0)
            {
                EndFaint();
            }
        }
    }

    void StartFaint()
    {
        onFaint?.Invoke();
        inactivatingGameObjects.ForEach(go => go.SetActive(false));
        disablingComponents.ForEach(comp => comp.enabled = false);
        
        health.invincible = true;
        
        inCache = col.includeLayers;
        exCache = col.excludeLayers;
        col.ExcludeAllLayers(ELayer.Bound);
        
        if (ani) ani.SetBool("isFaint", true);
        
        faintTimer = faintDuration;
    }
    
    void EndFaint()
    {
        onEndFaint?.Invoke();
        
        inactivatingGameObjects.ForEach(go => go.SetActive(true));
        disablingComponents.ForEach(comp => comp.enabled = true);
        
        health.ResetHealth();
        health.StartCD();
        health.OnDeath += StartFaint;
        
        if (ani) ani.SetBool("isFaint", false);
        
        col.includeLayers = inCache;
        col.excludeLayers = exCache;
    }
}