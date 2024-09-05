using System;
using UnityEngine;

public class OnEnterCon : ConditionBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Execute());
    }
}