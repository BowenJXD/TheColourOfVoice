using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : Singleton<CameraShakeManager>
{
    [SerializeField] private float globalShakeForce = 1f;
    private void Awake()
    {
        base.Awake();
    }

    public void CameraShake(CinemachineImpulseSource impulseSource) 
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}
