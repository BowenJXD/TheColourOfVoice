using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class LevelManager : Singleton<LevelManager>, ISetUp
{
    public LevelConfig config;
    
    public List<LevelConfig> levelConfigs;
    
    [ReadOnly] public int levelIndex = 1;
    [ReadOnly] public string levelName;
    [ReadOnly] public PaintColor levelColor;
    [ReadOnly] public Sprite tileSprite;
    [FormerlySerializedAs("enemyGenerator")] [ReadOnly] public EntityGenerator entityGenerator;
    [ReadOnly] public LevelMechanic mechanic;
    
    public GameObject backgroundParticleParent;
    public FMODUnity.StudioEventEmitter emitter;

    new void Awake()
    {
        levelIndex = PlayerPrefs.GetInt("levelIndex", levelIndex);
        //Debug.Log("LevelIndex is " + levelIndex);
        if (levelIndex < levelConfigs.Count)
        {
            ChangeConfig(levelIndex);
        }
    }
    
    /*private void Start()
    {
        if (ChoosingLevelData.NEXT_LEVEL_CONFIG < 0)
        {
            return;
        }
        ChangeConfig(ChoosingLevelData.NEXT_LEVEL_CONFIG);
    }*/

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
            entityGenerator = cfg.entityGenerator;
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
        if (entityGenerator)
        {
            Instantiate(entityGenerator, transform);
        }
        if (mechanic)
        {
            mechanic = Instantiate(mechanic, transform);
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
    
 
    
    [Header("PopUp Bubble"),ReadOnly] public PopUpBubble popUpBubblePrefab;
    
    /// <summary>
    /// 在需要的时候调用这个方法，传入对应的bubbledata
    /// </summary>
    /// <param name="bubbledata">对应data的名字</param>
    /// <returns></returns>
    public IEnumerator PopUpBubble(string bubbledata)
    {
        string path = $"BubbleData/{bubbledata}";
        var data = Resources.Load<PopUpData>(path);
        if (data == null)
        {
            Debug.Log($"PopUpData not found at path: BubbleData/{path}");
            yield break;
        }

        if (popUpBubblePrefab == null)
        {
            popUpBubblePrefab =GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PopupWindow"),GameObject.Find("PopupBubblePanel").transform) .GetComponent<PopUpBubble>();
            popUpBubblePrefab.Init();
        }
        popUpBubblePrefab.ShowBubble(data);
        
        yield return new WaitForSeconds(data.timeToDisplay);
        popUpBubblePrefab.CloseBubble();
        data.onFinishedEvent?.Invoke();
    }

    public void StartPopupBubble(string dataName)
    {
        StartCoroutine(PopUpBubble(dataName));
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