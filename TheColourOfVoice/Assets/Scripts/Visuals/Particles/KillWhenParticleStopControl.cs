using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillWhenParticleStopControl : MonoBehaviour
{
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (ps != null && ps.isStopped)
        Destroy(gameObject);
    }
}
