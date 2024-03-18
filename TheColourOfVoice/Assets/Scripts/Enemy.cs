using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Enemy : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    private Rigidbody2D rg;
    public ParticleSystem enemyHitTrailSmoke;

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    
    public void Damage(float damageAmount) 
    {
        CameraShakeManager.Instance.CameraShake(impulseSource);
    }

    public void KnockBack(Vector2 direction, float impluse) 
    {
        rg.AddForce(impluse * direction, ForceMode2D.Impulse);
        ParticleSystem dust = Instantiate(enemyHitTrailSmoke,this.transform);
        dust.transform.localPosition = new Vector3(0, 0, 0);
        dust.Play();
    }
}
