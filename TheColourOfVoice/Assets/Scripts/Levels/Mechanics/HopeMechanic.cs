using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HopeMechanic : LevelMechanic
{
    [ReadOnly]
    public int phase = 1;
    public Enemy bossPrefab;
    [HideInInspector] public Enemy boss;
    public ScoreBar scoreBar;
    public Health bossHealth;
    public Explosion explosion;
    public float phase2Score;
    public float[] phaseHealths;
    public List<EntityGenerator> enemyGenerators;
    public Sprite bossSprite;
    public float tileSaturation;
    public CinemachineTargetGroup targetGroup;
    public CinemachineImpulseSource impulseSource;
    public EntityGenerator expGenerator;

    private int damageCache;
    private EnterPhaseEffect enterPhaseEffect;
    Volume volume;
    Level_demo levelDemo;
    public RageMechanic rageMechanic;
    
    public override void Init()
    {
        base.Init();
        SplashGrid.Instance.tileSaturation = tileSaturation;
        boss = PoolManager.Instance.New(bossPrefab);
        scoreBar = FindObjectOfType<ScoreBar>();
        scoreBar.OnScoreChanged += OnScoreChanged;
        if (!bossHealth)
        {
            bossHealth = boss.GetComponent<Health>();
            enterPhaseEffect = boss.GetComponent<EnterPhaseEffect>();
        }

        if (!targetGroup)
        {
            targetGroup = FindObjectOfType<CinemachineTargetGroup>();
        }
        if (targetGroup)
        {
            targetGroup.AddMember(boss.transform, 0.5f, 1);
        }
        
        enemyGenerators = bossHealth.GetComponentsInChildren<EntityGenerator>(true).OrderBy(c => c.transform.GetSiblingIndex()).ToList();
        volume = FindObjectOfType<Volume>();
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            DOTween.To(() => colorAdjustments.saturation.value, x => colorAdjustments.saturation.value = x, -90, 2);
        }
        if (volume.profile.TryGet(out FilmGrain filmGrain))
        {
            DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x, 0.5f, 2);
        }
        levelDemo = FindObjectOfType<Level_demo>();
        levelDemo.SetChaosTimer(true);

        expGenerator = GetComponentInChildren<EntityGenerator>(true);
        
        Game.Instance.OnNextUpdate += () => BossBanner.Instance.ShowBanner(bossSprite, "Nionysus", ColorManager.Instance.GetColor(PaintColor.Red), true);
    }

    private void OnScoreChanged(float percentage)
    {
        if (scoreBar.score > phase2Score)
        {
            EnterPhase(2);
            scoreBar.OnScoreChanged -= OnScoreChanged;
        }
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.saturation.value = Mathf.Lerp(-90, -70, scoreBar.score / phase2Score);
        }
    }

    void EnterPhase(int newPhase)
    {
        phase = newPhase;
        switch (phase)
        {
            case 1:
                break;
            case 2:
                EnterPhase2();
                break;
            case 3:
                break;
            case 6:
                EnterPhase6();
                break;
            case 7:
                EnterPhase7();
                return;
            case 8:
                EnterPhase8();
                return;
        }

        if (explosion) 
        {
            Explosion exp = PoolManager.Instance.New(explosion);
            exp.transform.position = bossHealth.transform.position;
            exp.color = LevelManager.Instance.levelColor;
            exp.Init();
        }
        
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy.gameObject != bossHealth.gameObject)
            {
                enemy.Deinit();
            }
        }

        SpellManager.Instance.allSpells[^(phase - 1)].Upgrade();
        if (enemyGenerators.Count > phase - 2)
        {
            enemyGenerators[phase - 2].gameObject.SetActive(true);
        }
        
        enterPhaseEffect.EnterPhase(false);
    }

    void EnterPhase2()
    {
        int count = 0;
        foreach (SplashTile tile in SplashGrid.Instance.tiles)
        {
            if (tile.Color == PaintColor.Black)
            {
                tile.PaintTile(PaintColor.White);
                tile.OnPainted += OnTilePainted;
                count++;
            }
        }

        bossHealth.invincible = false;
        bossHealth.maxHealth = count;
        bossHealth.ResetHealth();
        bossHealth.OnHealthChanged += OnHealthChanged;
        
        scoreBar.maxScore = bossHealth.maxHealth;
        scoreBar.OnScoreChanged += f =>
        {
            scoreBar.score = bossHealth.currentHealth;
        };
        
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            DOTween.To(() => colorAdjustments.saturation.value, x => colorAdjustments.saturation.value = x, 0, 2);
        }
        if (volume.profile.TryGet(out FilmGrain filmGrain))
        {
            DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x, 0, 2);
        }
    }

    void EnterPhase6()
    {
        var obj = Instantiate(rageMechanic);
        obj.Init();
        obj.rageValue = obj.rageLimit;
        obj.rageDecay = 0;
        LevelManager.Instance.mechanic = obj;
    }
    
    private void EnterPhase7()
    {
        /*
         * 禁止boss动作
         * 禁止玩家移动和攻击
         * 播放爆炸特效
         * 屏幕抖动
         * 视角移动到boss身上
         * 
         * 特效结束后：
         * 视角移回玩家身上
         * 移回后：
         * 恢复玩家移动，攻击变成主动攻击
         * 添加bubble提示玩家按下攻击键
         * boss受击后会抖动，受击10次后炸裂
         * EnterPhase8
         */
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy.gameObject != bossHealth.gameObject)
            {
                enemy.Deinit();
            }
        }
        
        string[] texts = {"BD1", "BD2", "BD3", "BD4", "BD5", "BD6"};
        texts.OrderBy(s => s).ToArray();
        
        LevelManager.Instance.mechanic.Deinit();
        LevelManager.Instance.mechanic = null;
        
        enterPhaseEffect.EnterPhase(true);
        var player = GameObject.FindWithTag("Player");
        var move = player.GetComponent<PlayerMovement>();
        move.enabled = false;
        var fire = player.GetComponentInChildren<Fire>(true);
        fire.enabled = false;

        expGenerator = GetComponentInChildren<EntityGenerator>(true);
        expGenerator.spawnTransform = bossHealth.transform;
        expGenerator.gameObject.SetActive(true);
        expGenerator.enabled = true;
        
        CameraShakeManager.Instance.CameraShake(impulseSource);

        CinemachineTargetGroup.Target[] targets = new CinemachineTargetGroup.Target[2];
        Array.Copy(targetGroup.m_Targets, targets, 2);
        targetGroup.RemoveMember(player.transform);
        new LoopTask{interval = 3f, finishAction = () =>
        {
            targetGroup.m_Targets = targets;
            move.enabled = true;
            fire.enabled = true;
            fire.SetAutoFire(false);
            float damage = bossHealth.currentHealth / 7;
            fire.onFire += bullet =>
            {
                if (bullet.TryGetComponent<Attack>(out var attack))
                {
                    attack.damage = damage;
                }
            };
            bossHealth.damageCooldown = 0;
            LevelManager.Instance.StartPopupBubble("BD7");
        }}.Start();
    }
    
    void EnterPhase8()
    {
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            levelDemo.EndLevel(() =>
            {
                DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, 8, 2)
                    .OnComplete(() => SceneTransit.Instance.LoadTargetScene("Ending"));
            });
        }
        
        boss.Deinit();
    }
    
    private bool OnTilePainted(Painter obj)
    {
        if (obj.paintColor is PaintColor.Black or PaintColor.White or PaintColor.Null) return false;
        damageCache += 1;
        if (!bossHealth.invincible)
        {
            bossHealth.AlterHealth(-damageCache);
            damageCache = 0;
        }

        return true;
    }
    
    void OnHealthChanged(float newHealth)
    {
        if (phase - 2 >= phaseHealths.Length) return;
        if (bossHealth.GetHealthPercentage() * 100 < phaseHealths[phase - 2])
        {
            EnterPhase(phase + 1);
        }
    }

    void EnterPhase3()
    {
        
    }
}