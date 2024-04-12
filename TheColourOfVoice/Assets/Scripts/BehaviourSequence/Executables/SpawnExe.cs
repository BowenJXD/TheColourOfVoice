using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExe : ExecutableBehaviour
{
    public float offset = 0.5f;
    public float force = 10f;
    public List<float> angles = new() { 90f, -90f };

    public Entity prefab;

    private void Split()
    {
        foreach (var angle in angles)
        {
            Vector3 direction = Util.GetVectorFromAngle(angle);
            Vector3 spawningPosition = transform.position + direction * offset;
            var entity = PoolManager.Instance.New(prefab);
            entity.transform.position = spawningPosition;
            entity.Init();
            if (force != 0 && entity.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }

    public override IEnumerator Execute(IExecutor blackboard)
    {
        Split();
        yield return null;
    }
}