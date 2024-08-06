using UnityEngine;

/// <summary>
/// Based on <see cref="ChaseMovement"/>
/// </summary>
public class ChaseShoot : Shooter
{
    ChaseMovement chaseMovement;

    public override void SetUp()
    {
        base.SetUp();
        chaseMovement = GetComponent<ChaseMovement>();
        chaseMovement.OnEnterRange += StartShoot;
        chaseMovement.OnExitRange += StopShoot;
    }

    protected override float GetAngle()
    {
        if (!chaseMovement || !chaseMovement.target) return base.GetAngle();

        Vector2 distance = (chaseMovement.target.transform.position - transform.position);
        return distance.GetAngle();
    }
}