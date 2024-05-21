using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTransition : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;

    private void Awake()
    {
        throw new NotImplementedException();
    }

    public void ShowUI()
    {
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        float currentTime = 0;
        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, Vector3.zero, Mathf.Clamp01(currentTime/ transitionTime));
            yield return null;
        }
    }
}
