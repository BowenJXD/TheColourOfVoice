using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExe : ExecutableBehaviour
{
    public float offset = 0.5f;
    public float force = 10f;
    public List<float> angles = new() { 90f, -90f };
    public bool doSetRotation = false;
    public bool doUseBBAngles = false;
    public bool doWaitForAllToDeinit = false;
    private int entsToDeinit = 0;

    public Entity prefab;

    protected override void OnStart()
    {
        base.OnStart();
        if (doUseBBAngles)
        {
            if (blackboard.TryGet<float[]>(BBKey.ANGLES, out var angles))
            {
                this.angles = new List<float>(angles);
            }
        }

        if (doWaitForAllToDeinit && angles.Count > 0)
        {
            StartExe();
        }
        Split();
    }

    private void Split()
    {
        for (int i = 0; i < angles.Count; i++)
        {
            float angle = angles[i];
            
            Vector3 direction = Util.GetVectorFromAngle(angle);
            Vector3 spawningPosition = transform.position + direction * offset;
            var entity = PoolManager.Instance.New(prefab);
            entity.transform.position = spawningPosition;
            if (doSetRotation)
            {
                entity.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            
            entity.Init();
            if (force != 0 && entity.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }

            if (doWaitForAllToDeinit)
            {
                entsToDeinit++;
                entity.onDeinit += OnDeinit;
            }
        }
    }
    
    void OnDeinit()
    {
        if (doWaitForAllToDeinit)
        {
            entsToDeinit--;
            if (entsToDeinit == 0)
            {
                FinishExe();
            }
        }
    }
}