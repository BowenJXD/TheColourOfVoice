using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RageMechanic : LevelMechanic
{
    [ReadOnly] public float rageValue = 0;
    public float rageLimit;
    public float detectionInterval = 1;
    public float rageDecay;
    [ReadOnly] public bool isRageMode;
    public GameObject rageEffect;
    
    LoopTask loopTask;

    public override void Init()
    {
        rageValue = 0;
        loopTask = new LoopTask{loopAction = IncreaseRage, interval = 1, loop = -1};
        loopTask.Start();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isRageMode) return;
        rageValue -= rageDecay * Time.deltaTime;
        if (rageValue <= 0)
        {
            ExitRageMode();
        }
    }

    void IncreaseRage()
    {
        float volume = VolumeDetection.Instance.GetPeakVolume(detectionInterval);
        rageValue += volume;
        if (!isRageMode && rageValue >= rageLimit)
        {
            EnterRageMode();
        }
    }

    /// <summary>
    /// Reset on deinit
    /// </summary>
    public Action OnEnterRage;
    
    void EnterRageMode()
    {
        rageValue = rageLimit;
        isRageMode = true;
        
        List<Enemy> enemies = PoolManager.Instance.FindAll<Enemy>();
        foreach (var enemy in enemies)
        {
            SetUpRage(enemy);
        }
        PoolManager.Instance.AddGetAction<Enemy>(SetUpRage);
        
        rageEffect.SetActive(true);
        OnEnterRage?.Invoke();
    }

    void SetUpRage(Entity enemy)
    {
        if (enemy.TryGetComponent(out Health health))
        {
            health.invincible = true;
            enemy.onDeinit += () => FinishRage(enemy);
        }
        if (enemy.TryGetComponent(out RageBehaviour rageBehaviour))
        {
            rageBehaviour.Ignite();
        }
    }

    /// <summary>
    /// Reset on deinit
    /// </summary>
    public Action OnExitRage;
    
    void ExitRageMode()
    {
        rageValue = 0;
        isRageMode = false;
        
        List<Enemy> enemies = PoolManager.Instance.FindAll<Enemy>();
        foreach (var enemy in enemies)
        {
            FinishRage(enemy);
        }
        PoolManager.Instance.RemoveGetAction<Enemy>(SetUpRage);
        
        rageEffect.SetActive(false);
        OnExitRage?.Invoke();
    }
    
    void FinishRage(Entity enemy)
    {
        if (enemy.TryGetComponent(out Health health))
        {
            health.invincible = false;
        }
    }
    
    public override void Deinit()
    {
        OnEnterRage = null;
        OnExitRage = null;
    }
}