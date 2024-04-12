using UnityEngine;

public class Explosion : MonoBehaviour, ISetUp
{
    public int particleCount;
    public float angleVariation;
    public float angleOffset;
    public float offsetVariation;
    float[] angles;
    
    public float speed;
    public float speedVariation;
    
    public float duration;
    public float durationVariation;
    
    public PaintColor color;
    public float scale;
    
    public BulletBase bulletPrefab;

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        PoolManager.Instance.Register(bulletPrefab);
        angles = GetAngles();
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }
    
    public void Execute()
    {
        if (angleVariation > 0) angles =  GetAngles();
        for (int i = 0; i < particleCount; i++)
        {
            BulletBase bullet = PoolManager.Instance.New(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angles[i]);
            bullet.transform.localScale = Vector3.one * scale;
            bullet.speed = speed + Random.Range(-speedVariation, speedVariation);
            bullet.duration = duration + Random.Range(-durationVariation, durationVariation);
            bullet.GetComponent<Painter>()?.SetColor(color);
            bullet.Init();
            bullet.SetDirection(bullet.transform.up);
        }
    }
    
    float[] GetAngles()
    {
        angles = new float[particleCount];  
        float offset = angleOffset + Random.Range(-offsetVariation, offsetVariation);
        for (int i = 0; i < particleCount; i++)
        {
            float baseAngle = 360 / particleCount * i + offset;
            float variedAngle = Random.Range(baseAngle - angleVariation, baseAngle + angleVariation);
            angles[i] = variedAngle;
        }
        return angles;
    }
}