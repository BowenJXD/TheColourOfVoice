using System;
using UnityEngine;

public class ParticleRangeControl : MonoBehaviour, ISetUp
{
    public bool update = false;
    public float scaleRadiusRatio = 0f;

    public Transform target;
    public ParticleSystem ps;

    private float radiusCache;
    float scaleCache;
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        var shape = ps.shape;
        radiusCache = shape.radius;
        scaleCache = target.localScale.x;
        if (scaleRadiusRatio == 0) scaleRadiusRatio = scaleCache / radiusCache;
        shape.radius = scaleCache / scaleRadiusRatio;
    }

    private void Update()
    {
        if (update && target.localScale.x != scaleCache)
        {
            scaleCache = target.localScale.x;
            var shape = ps.shape;
            shape.radius = scaleCache / scaleRadiusRatio;
        }
    }

    private void OnDisable()
    {
        var shape = ps.shape;
        shape.radius = radiusCache;
    }

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        if (!target) target = transform;
        if (!ps) ps = GetComponent<ParticleSystem>();
    }
}