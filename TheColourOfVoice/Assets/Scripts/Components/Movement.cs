using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;

    /// <summary>
    /// Never reset automatically
    /// </summary>
    public Action<Vector2> OnMove;
}