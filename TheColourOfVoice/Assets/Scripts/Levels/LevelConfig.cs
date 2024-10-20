using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int levelIndex;
    public string levelName;
    public PaintColor levelColor;
    public Sprite tileSprite;
    [FormerlySerializedAs("enemyGenerator")] public EntityGenerator entityGenerator;
    public LevelMechanic mechanic;
}