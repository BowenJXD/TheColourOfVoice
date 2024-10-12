using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RageMechanic : LevelMechanic
{
    [ReadOnly] public float rageValue = 0;
    public float rageLimit;
    public float detectionInterval = 1;
    public float rageDecay;
    [ReadOnly] public bool isRageMode;
    public GameObject rageEffect;
    public float fadeTime = 0.5f;
    Image[] rageEffectImages;
    
    LoopTask loopTask;
    BurningBuff burningBuff;

    public override void Init()
    {
        rageValue = 0;
        if (!rageEffect) rageEffect = Resources.Load<GameObject>("Prefabs/RageEdge");
        rageEffect = Instantiate(rageEffect, FindObjectOfType<Canvas>().transform);
        rageEffectImages = rageEffect.GetComponentsInChildren<Image>(true);
        loopTask = new LoopTask{loopAction = IncreaseRage, interval = 1, loop = -1};
        loopTask.Start();
        
        burningBuff = GetComponentInChildren<BurningBuff>(true);
        if (burningBuff) burningBuff.Init();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Lebug.Log("Rage Value", rageValue, "Rage");
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
        foreach (var image in rageEffectImages)
        {
            float alpha = image.color.a;
            image.color -= new Color(0, 0, 0, alpha);
            image.DOFade(alpha, fadeTime).Play();
        }
        
        OnEnterRage?.Invoke();
    }

    void SetUpRage(Entity enemy)
    {
        if (enemy.TryGetComponent(out RageBehaviour rageBehaviour))
        {
            if (rageBehaviour.isRage) return;
            if (enemy.TryGetComponent(out BuffOwner buffOwner))
            {
                buffOwner.ApplyBuff(burningBuff);
                buffOwner.OnBuffReceived += (buff) =>
                {
                    if (buff is FreezeBuff freezeBuff)
                    {
                        FinishRage(enemy);
                    }
                };
            }
        }
        if (enemy.TryGetComponent(out Health health))
        {
            health.invincible = true;
            enemy.onDeinit += () => FinishRage(enemy);
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
        
        foreach (var image in rageEffectImages)
        {
            float alpha = image.color.a;
            image.DOFade(0, fadeTime).OnComplete(() =>
            {
                image.color += new Color(0, 0, 0, alpha);
                image.gameObject.SetActive(false);
            }).Play();
        }
        
        OnExitRage?.Invoke();
    }
    
    void FinishRage(Entity enemy)
    {
        if (enemy.TryGetComponent(out Health health))
        {
            health.invincible = false;
        }

        if (enemy.TryGetComponent(out BuffOwner owner))
        {
            owner.OnBuffReceived -= (buff) =>
            {
                if (buff is FreezeBuff freezeBuff)
                {
                    FinishRage(enemy);
                }
            };
        }
    }
    
    public override void Deinit()
    {
        ExitRageMode();
        loopTask.Stop();
        OnEnterRage = null;
        OnExitRage = null;
    }
}