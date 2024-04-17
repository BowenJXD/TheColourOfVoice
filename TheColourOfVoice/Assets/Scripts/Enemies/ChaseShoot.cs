using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Based on <see cref="ChaseMovement"/>
/// </summary>
public class ChaseShoot : MonoBehaviour, ISetUp
{
    public float shootInterval = 1;
    public BulletBase bulletPrefab;
    public float offset = 1;
    public float spread;
    public float recoil;
    
    LoopTask shootTask;
    ChaseMovement chaseMovement;
    Rigidbody2D rb;

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        rb = GetComponent<Rigidbody2D>();
        chaseMovement = GetComponent<ChaseMovement>();
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
        chaseMovement.OnEnterRange += StartShoot;
        chaseMovement.OnExitRange += StopShoot;
    }

    private void StartShoot()
    {
        shootTask.Resume();
    }

    void Shoot()
    {
        if (!chaseMovement || !chaseMovement.target) return;

        Vector2 distance = (chaseMovement.target.transform.position - transform.position);
        float angle = distance.GetAngle();
        float actualSpread = Random.Range(-spread, spread);
        Vector2 direction = Util.GetVectorFromAngle(angle + actualSpread);
        
        var bullet = PoolManager.Instance.New(bulletPrefab);
        bullet.transform.position = transform.position + (Vector3) (direction * offset);
        bullet.Init();
        bullet.SetDirection(direction);
        if (rb) rb.AddForce(-direction * recoil, ForceMode2D.Impulse);
    }
    
    private void StopShoot()
    {
        shootTask.Pause();
    }

}