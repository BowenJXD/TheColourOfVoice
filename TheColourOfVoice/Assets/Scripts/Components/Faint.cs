using System;
using UnityEngine;

/// <summary>
/// Faint system for game objects, will freeze the game object for a certain amount of time when dead and reset <see cref="Health"/>
/// </summary>
public class Faint : MonoBehaviour, ISetUp
{
    public float faintDuration;

    private LoopTask loopTask;
    private Health health;
    private Rigidbody2D rg;
    RigidbodyConstraints2D cachedConstraints;
    
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
        rg = GetComponent<Rigidbody2D>();
    }
    
    void StartFaint()
    {
        cachedConstraints = rg.constraints;
        rg.constraints = RigidbodyConstraints2D.FreezeAll;
        loopTask.Start();
    }
    
    void EndFaint()
    {
        rg.constraints = cachedConstraints;
        health.ResetHealth();
    }
}