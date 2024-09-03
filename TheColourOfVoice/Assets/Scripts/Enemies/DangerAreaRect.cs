using System;
using UnityEngine;

public class DangerAreaRect : ParticleEntity
{
    public Vector2 size;
    public ParticleSystem borderParticle;
    public SpriteRenderer areaSprite;

    private void OnValidate()
    {
        Set();
    }

    private void OnEnable()
    {
        Set();
    }

    void Set()
    {
        if (borderParticle)
        {
            borderParticle.transform.localPosition = new Vector3(size.x / 2, 0, 0);
            var shape = borderParticle.shape;
            shape.scale = new Vector3(size.x, size.y, 1);
        }

        if (ps)
        {
            var module = ps.velocityOverLifetime;
            module.speedModifierMultiplier = size.x;
            var main = ps.main;
            main.startSize = size.y;
        }
        
        if (areaSprite)
        {
            areaSprite.size = size;
        }
    }
}