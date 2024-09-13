using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HopeMechanic : LevelMechanic
{
    [ReadOnly]
    public int phase = 1;
    public ScoreBar scoreBar;
    public Health bossHealth;
    public Explosion explosion;
    public float phase2Score;
    public float[] phaseHealths;
    public List<EnemyGenerator> enemyGenerators;
    public Sprite bossSprite;

    private int damageCache;
    private EnterPhaseEffect enterPhaseEffect;
    Volume volume;
    Level_demo levelDemo;
    public RageMechanic rageMechanic;
    
    public override void Init()
    {
        base.Init();
        scoreBar = FindObjectOfType<ScoreBar>();
        scoreBar.OnScoreChanged += OnScoreChanged;
        if (!bossHealth)
        {
            var enemy = GameObject.FindWithTag("Enemy");
            bossHealth = enemy.GetComponent<Health>();
            enterPhaseEffect = enemy.GetComponent<EnterPhaseEffect>();
        }
        enemyGenerators = FindObjectsOfType<EnemyGenerator>(true).OrderBy(c => c.transform.GetSiblingIndex()).ToList();
        volume = FindObjectOfType<Volume>();
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            DOTween.To(() => colorAdjustments.saturation.value, x => colorAdjustments.saturation.value = x, -100, 2);
        }
        if (volume.profile.TryGet(out FilmGrain filmGrain))
        {
            DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x, 0.5f, 2);
        }
        levelDemo = FindObjectOfType<Level_demo>();
        levelDemo.SetChaosTimer(true);
        
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
            colorAdjustments.saturation.value = Mathf.Lerp(-100, -90, scoreBar.score / phase2Score);
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
        
        enterPhaseEffect.EnterPhase(phase);
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
        LevelManager.Instance.mechanic = obj;
    }
    
    void EnterPhase7()
    {
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            levelDemo.EndLevel(() =>
            {
                DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, 8, 2)
                    .OnComplete(Application.Quit);
            });
        }
        
        enterPhaseEffect.EnterPhase(phase);
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