﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISetUp
{
    public LevelConfig config;
    
    [ReadOnly] public PaintColor levelColor;
    [ReadOnly] public Sprite tileSprite;
    [ReadOnly] public EnemyGenerator enemyGenerator;
    [ReadOnly] public LevelMechanic mechanic;
    
    public GameObject backgroundParticleParent;

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
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
            levelColor = config.levelColor;
            tileSprite = config.tileSprite;
            enemyGenerator = config.enemyGenerator;
            mechanic = config.mechanic;
        }
    }

    public void Init()
    {
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
                var cachedAlphaKeys = startColor.gradient.alphaKeys;
                startColor.gradient = ColorManager.Instance.GetGradient(levelColor);
                startColor.gradient.alphaKeys = cachedAlphaKeys;
                main.startColor = startColor;
                ps.Play();
            }
        }
    }
    
    void Update()
    {
        mechanic.OnUpdate();
    }
    
    private void OnDisable()
    {
        mechanic.Deinit();
    }
}