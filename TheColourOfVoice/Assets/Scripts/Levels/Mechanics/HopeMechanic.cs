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

    private int damageCache;
    
    public override void Init()
    {
        base.Init();
        if (!scoreBar) scoreBar = FindObjectOfType<ScoreBar>();
        scoreBar.OnScoreChanged += OnScoreChanged;
        if (!bossHealth) bossHealth = GameObject.FindWithTag("Enemy").GetComponent<Health>();
    }

    private void OnScoreChanged(float percentage)
    {
        if (scoreBar.score > phase2Score)
        {
            EnterPhase(2);
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
        }

        if (explosion) 
        {
            Explosion exp = PoolManager.Instance.New(explosion);
            exp.transform.position = bossHealth.transform.position;
            exp.color = LevelManager.Instance.levelColor;
            exp.Init();
        }
    }
    
    void EnterPhase2()
    {
        foreach (SplashTile tile in SplashGrid.Instance.tiles)
        {
            if (tile.Color == PaintColor.Black)
            {
                tile.PaintTile(PaintColor.White);
                tile.OnPainted += OnTilePainted;
            }
        }

        bossHealth.invincible = false;
        bossHealth.TakeDamageAfter += TakeDamageAfter;

        SpellManager.Instance.allSpells[^(phase - 1)].Upgrade();
    }

    private void OnTilePainted(Painter obj)
    {
        if (bossHealth.invincible)
        {
            damageCache += 1;
        }
        else 
        {
            bossHealth.TakeDamage(damageCache);
            damageCache = 0;
        }
    }
    
    void TakeDamageAfter(float dmg)
    {
        if (bossHealth.GetHealthPercentage() < phaseHealths[phase - 2])
        {
            EnterPhase(phase + 1);
        }
    }

    void EnterPhase3()
    {
        
    }
}