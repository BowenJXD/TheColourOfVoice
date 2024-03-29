using System;
using System.Linq;
using UnityEngine;

public class Game : Singleton<Game>
{
    /// <summary>
    /// Reset on trigger
    /// </summary>
    public Action OnNextUpdate;
    
    protected override void Awake()
    {
        base.Awake();
        // Initialize the game
    }

    private void Update()
    {
        OnNextUpdate?.Invoke();
        OnNextUpdate = null;
    }
}