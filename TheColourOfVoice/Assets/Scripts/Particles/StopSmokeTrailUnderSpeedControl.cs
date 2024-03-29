using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSmokeTrailUnderSpeedControl : MonoBehaviour, ISetUp
{
    public float minVelocity = 0.1f;
    public float initTime = 0.2f;
    private float initTimer = 0.2f;

    private ParticleController smokeTrailParticleSystem;
    private Vector2 lastPos;

    public bool IsSet { get; set; }

    public void SetUp()
    {
        IsSet = true;
        smokeTrailParticleSystem = GetComponent<ParticleController>();
    }

    private void OnEnable()
    {
        if (!IsSet) SetUp();
        lastPos = new Vector2(0, 0);
        initTimer = initTime;
    }

    private void FixedUpdate()
    {
        if (initTimer > 0)
        {
            initTimer -= Time.deltaTime;
        }
        else
        {
            float d = ((Vector2)transform.position - lastPos).magnitude;
            float v = d / Time.fixedDeltaTime;
            if (v < minVelocity)
            {
                smokeTrailParticleSystem.Deinit();
            }
        }

        lastPos = transform.position;
    }
}
