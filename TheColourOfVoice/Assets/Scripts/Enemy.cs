using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Enemy : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Damage(float damageAmount) 
    {
        CameraShakeManager.Instance.CameraShake(impulseSource);
    }
}
