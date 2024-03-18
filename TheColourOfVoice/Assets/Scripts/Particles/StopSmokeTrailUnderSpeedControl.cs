using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSmokeTrailUnderSpeedControl : MonoBehaviour
{
    public float minVelocity = 0.1f;
    public float initTime = 0.2f;

    private ParticleSystem smokeTrailParticalSystem;
    private Vector2 lastPos;

    private void Start()
    {
        smokeTrailParticalSystem = GetComponent<ParticleSystem>();
        lastPos = new Vector2(0,0);

    }

    private void FixedUpdate()
    {
        if (initTime >0)
        {
            initTime -= Time.deltaTime;
        }
        else
        {
            float d = ((Vector2)transform.position - lastPos).magnitude;
            float v = d / Time.fixedDeltaTime;
            if (v < minVelocity)
            {
                smokeTrailParticalSystem.Stop();
                Destroy(this);
            }
        }

        lastPos = transform.position;
    }
}
