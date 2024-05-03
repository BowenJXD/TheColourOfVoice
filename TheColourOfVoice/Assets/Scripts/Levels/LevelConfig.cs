using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Level/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public PaintColor levelColor;
    public Sprite tileSprite;
    public EnemyGenerator enemyGenerator;
    public LevelMechanic mechanic;
}