using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Faint system for game objects, will freeze the game object for a certain amount of time when dead and reset <see cref="Health"/>
/// </summary>
public class Faint : MonoBehaviour, ISetUp
{
    public float faintDuration;

    private LoopTask loopTask;
    public List<GameObject> inactivatingGameObjects;
    public List<Behaviour> disablingComponents;
    private Health health;
    private Collider2D col;
    private LayerMask inCache;
    private LayerMask exCache;
    public Animator ani;

    private void OnEnable()
    {
        if (!IsSet) SetUp();
        health.OnDeath += StartFaint;
        loopTask = new LoopTask { interval = faintDuration, loop = 1, finishAction = EndFaint };
    }

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        health = GetComponent<Health>();
        col = GetComponent<Collider2D>();
    }
    
    void StartFaint()
    {
        inactivatingGameObjects.ForEach(go => go.SetActive(false));
        disablingComponents.ForEach(comp => comp.enabled = false);
        
        health.invincible = true;
        
        inCache = col.includeLayers;
        exCache = col.excludeLayers;
        col.ExcludeAllLayers(ELayer.Bound);
        
        if (ani) ani.SetBool("isFaint", true);
        
        loopTask.Start();
    }
    
    void EndFaint()
    {
        inactivatingGameObjects.ForEach(go => go.SetActive(true));
        disablingComponents.ForEach(comp => comp.enabled = true);
        
        health.StartCD();
        health.OnDeath += StartFaint;
        
        if (ani) ani.SetBool("isFaint", false);
        
        col.includeLayers = inCache;
        col.excludeLayers = exCache;
    }
}