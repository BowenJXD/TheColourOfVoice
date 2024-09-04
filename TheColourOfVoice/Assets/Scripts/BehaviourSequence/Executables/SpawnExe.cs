using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public enum SpawnMode
{
    Radial,
    Range,
}

public class SpawnExe : ExecutableBehaviour
{
    public SpawnMode mode = SpawnMode.Radial;
    [ShowIf("mode", SpawnMode.Radial)]
    public float offset = 0.5f;
    public float force = 10f;
    [ShowIf("mode", SpawnMode.Radial)]
    public List<float> angles = new() { 90f, -90f };
    private List<float> actualAngles = new();
    [ShowIf("mode", SpawnMode.Range)] 
    public List<Vector2> ranges = new() { new Vector3(0, 0, 0) };
    [ShowIf("mode", SpawnMode.Range)]
    public int randomCount = 0;
    [ShowIf("mode", SpawnMode.Range)]
    public Vector2 range = new Vector2(0, 0);
    [ShowIf("mode", SpawnMode.Range)]
    public bool useWorldSpace = false;
    public bool doSetRotation = false;
    public bool doUseBBValues = false;
    public bool doWaitForAllToDeinit = false;
    private int entsToDeinit = 0;

    public Entity prefab;

    protected override void OnStart()
    {
        base.OnStart();
        actualAngles = new List<float>(angles);
        if (doUseBBValues)
        {
            if (blackboard.TryGet<float>(BBKey.BASE_ANGLE, out var angle))
            {
                for (int i = 0; i < actualAngles.Count; i++)
                {
                    actualAngles[i] += angle;
                }
            }
        }

        if (doWaitForAllToDeinit && actualAngles.Count > 0)
        {
            StartExe();
        }
        
        foreach (var direction in GetSpawnLocation())
        {
            Spawn(direction);
        }
    }

    private void Spawn(Vector2 direction)
    {
        Vector3 spawningPosition = direction;
        if (mode == SpawnMode.Radial) spawningPosition *= offset;
        if (!useWorldSpace) spawningPosition += transform.position;
        var entity = PoolManager.Instance.New(prefab);
        entity.transform.position = spawningPosition;
        if (doSetRotation)
        {
            entity.transform.rotation = Quaternion.Euler(0, 0, direction.GetAngle());
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
    
    List<Vector2> GetSpawnLocation()
    {
        if (mode == SpawnMode.Radial && actualAngles.Count > 0)
        {
            return actualAngles.ConvertAll(angle => Util.GetVectorFromAngle(angle));
        }
        if (mode == SpawnMode.Range)
        {
            if (randomCount > 0)
            {
                List<Vector2> randomRanges = new();
                for (int i = 0; i < randomCount; i++)
                {
                    randomRanges.Add(new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y)));
                }
                return randomRanges;
            }
            return ranges;
        }
        return new List<Vector2>();
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