using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int levelIndex;
    public string levelName;
    public PaintColor levelColor;
    public Sprite tileSprite;
    public EnemyGenerator enemyGenerator;
    public LevelMechanic mechanic;
}