using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class HurtVignette : MonoBehaviour
{
    Vignette vignette;
    public float minIntensity;
    public float maxIntensity;
    public float maxIntensityDamage;
    public float fadeSpeed;
    float fadeVelocity;
    Health playerHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        Volume volumeComp = GetComponent<Volume>();
        VolumeProfile profile = volumeComp.sharedProfile;
        profile.TryGet(out vignette);
        if (!playerHealth)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        playerHealth.TakeDamageAfter += ShowVignette;
    }

    // Update is called once per frame
    void Update()
    {
        if (vignette.intensity.value > 0)
        {
            vignette.intensity.value -= fadeVelocity * Time.deltaTime;
            fadeVelocity += Time.deltaTime;
        }
        else
        {
            fadeVelocity = fadeSpeed;
        }
    }

    public void ShowVignette(float damage)
    {
        if (damage <= 0) return;
        float intensityPercentage = Mathf.Clamp01(damage / maxIntensityDamage);
        vignette.intensity.value = Mathf.Lerp(minIntensity, maxIntensity, intensityPercentage);
    }

    private void OnDestroy()
    {
        vignette.intensity.value = 0;
    }
}