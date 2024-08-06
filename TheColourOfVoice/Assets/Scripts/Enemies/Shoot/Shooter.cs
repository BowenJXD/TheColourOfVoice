using UnityEngine;

public class Shooter : MonoBehaviour, ISetUp
{
    public float shootInterval = 1;
    public BulletBase bulletPrefab;
    [Tooltip("0: right, 90: up, 180: left, 270: down")]
    public float angle = 0;
    public float offset = 1;
    public float spread;
    public float recoil;
    
    protected LoopTask shootTask;
    Rigidbody2D rb;

    public bool IsSet { get; set; }
    public virtual void SetUp()
    {
        IsSet = true;
        rb = GetComponent<Rigidbody2D>();
        shootTask = new LoopTask
        {
            interval = shootInterval,
            loopAction = Shoot,
            loop = -1,
        };
        shootTask.Start(false);
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
    }

    public void StartShoot()
    {
        shootTask.Resume();
    }
    
    protected virtual float GetAngle()
    {
        return angle;
    }

    void Shoot()
    {
        float actualAngle = GetAngle();
        float actualSpread = Random.Range(-spread, spread);
        Vector2 direction = Util.GetVectorFromAngle(actualAngle + actualSpread);
        
        var bullet = PoolManager.Instance.New(bulletPrefab);
        bullet.transform.position = transform.position + (Vector3) (direction * offset);
        bullet.Init();
        bullet.SetDirection(direction);
        if (recoil > 0 && rb) rb.AddForce(-direction * recoil, ForceMode2D.Impulse);
    }
    
    public void StopShoot()
    {
        shootTask.Pause();
    }

    private void OnDisable()
    {
        shootTask.Pause();
    }

    public void SetInterval(float newInterval)
    {
        shootInterval = newInterval;
        bool isPlaying = shootTask.isPlaying;
        shootTask.Stop();
        shootTask = new LoopTask
        {
            interval = shootInterval,
            loopAction = Shoot,
            loop = -1,
        };
        shootTask.Start(isPlaying);
    }
}