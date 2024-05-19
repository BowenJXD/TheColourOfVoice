using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISetUp
{
    public LevelConfig config;
    
    [ReadOnly] public int levelIndex;
    [ReadOnly] public PaintColor levelColor;
    [ReadOnly] public Sprite tileSprite;
    [ReadOnly] public EnemyGenerator enemyGenerator;
    [ReadOnly] public LevelMechanic mechanic;
    
    public GameObject backgroundParticleParent;
    public FMODUnity.StudioEventEmitter emitter;

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        if (!emitter) emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        Init();
    }

    private void OnValidate()
    {
        if (config)
        {
            levelIndex = config.levelIndex;
            levelColor = config.levelColor;
            tileSprite = config.tileSprite;
            enemyGenerator = config.enemyGenerator;
            mechanic = config.mechanic;
        }
    }

    public void Init()
    {
        if (levelIndex != 0)
        {
            if (emitter) emitter.SetParameter("LevelIndex", levelIndex);
        }
        if (enemyGenerator)
        {
            Instantiate(enemyGenerator, transform);
        }
        if (mechanic)
        {
            Instantiate(mechanic, transform);
            mechanic.Init();
        }
        if (backgroundParticleParent)
        {
            var psList = backgroundParticleParent.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in psList)
            {
                var main = ps.main;
                var startColor = main.startColor;
                startColor.gradient.colorKeys = ColorManager.Instance.GetGradient(levelColor).colorKeys;
                main.startColor = startColor;
                ps.Play();
            }
        }
    }
    
    void Update()
    {
        if (mechanic) mechanic.OnUpdate();
    }
    
    private void OnDisable()
    {
        if (mechanic) mechanic.Deinit();
    }
}