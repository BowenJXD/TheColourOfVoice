using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

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

    private int damageCache;
    private EnterPhaseEffect enterPhaseEffect;
    
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
    }

    private void OnScoreChanged(float percentage)
    {
        if (scoreBar.score > phase2Score)
        {
            EnterPhase(2);
            scoreBar.OnScoreChanged -= OnScoreChanged;
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
            case 7:
                enterPhaseEffect.EnterPhase(phase);
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
        if (enemyGenerators.Count >= phase)
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
        if (bossHealth.GetHealthPercentage() * 100 < phaseHealths[phase - 2])
        {
            EnterPhase(phase + 1);
        }
    }

    void EnterPhase3()
    {
        
    }
}