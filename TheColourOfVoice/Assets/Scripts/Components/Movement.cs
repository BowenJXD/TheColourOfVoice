﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    public float speed;

    [Tooltip("The weight of the input direction.")]
    public float inputWeight = 0.5f;
    
    public Dictionary<string, float> speedModifiers = new();
    
    public float Speed
    {
        get
        {
            float result = speed;
            foreach (var modifier in speedModifiers)
            {
                result *= modifier.Value;
            }

            return result;
        }
        set { speed = value; }
    }

    /// <summary>
    /// Never reset automatically
    /// </summary>
    public Action<Vector2> OnMove;
}