using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private Material screenTransitionMaterial;

    [SerializeField] private float transitionTime = 1f;
    
    [SerializeField] string propertyName = "_Progress";

    public UnityEvent OnTransitionDone;

    private void Start()
    {
        //Invoke("StartTransition", 2f);
        StartCoroutine(TransitionCoroutine());
    }
    
    public void StartTransition()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        float currentTime = 0;
        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;
            screenTransitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime/ transitionTime));
            yield return null;
        }
        OnTransitionDone?.Invoke();
    }
}
