using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISetUp
{
    public LevelConfig config;
    
    public List<LevelConfig> levelConfigs;
    
    [ReadOnly] public int levelIndex = 1;
    [ReadOnly] public string levelName;
    [ReadOnly] public PaintColor levelColor;
    [ReadOnly] public Sprite tileSprite;
    [ReadOnly] public EnemyGenerator enemyGenerator;
    [ReadOnly] public LevelMechanic mechanic;
    
    public GameObject backgroundParticleParent;
    public FMODUnity.StudioEventEmitter emitter;

    new void Awake()
    {
        levelIndex = PlayerPrefs.GetInt("levelIndex", levelIndex);
        if (levelIndex < levelConfigs.Count)
        {
            ChangeConfig(levelIndex);
        }
    }
    
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
            ChangeConfig(config);
        }
    }

    public void ChangeConfig(int index)
    {
        if (index < levelConfigs.Count)
        {
            ChangeConfig(levelConfigs[index]);
        }
    }
    
    void ChangeConfig(LevelConfig cfg)
    {
        if (cfg)
        {
            levelIndex = cfg.levelIndex;
            levelName = cfg.levelName;
            levelColor = cfg.levelColor;
            tileSprite = cfg.tileSprite;
            enemyGenerator = cfg.enemyGenerator;
            mechanic = cfg.mechanic;
        }
    }

    public void Init()
    {
        if (levelIndex != 0)
        {
            if (emitter) emitter.SetParameter("LevelIndex", levelIndex);
        }
        if (levelName != null)
        {
            TextPainter.Instance.text = levelName;
            TextPainter.Instance.color = levelColor;
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