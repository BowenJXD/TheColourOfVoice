using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

/// <summary>
/// A painter that paints the tiles in the splash grid with a specific color.
/// </summary>
public class Painter : MonoBehaviour, ISetUp
{
    public PaintColor paintColor;
    List<Vector2Int> cellIndexes = new List<Vector2Int>();
    
    ParticleSystem ps;
    
    /// <summary>
    /// Reset on disable.
    /// </summary>
    public Action<SplashTile> OnPaint;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;
        if (col.TryGetComponent(out SplashTile tile))
        {
            if (!cellIndexes.Contains(tile.CellIndex))
            {
                if (tile.PaintTile(this))
                {
                    OnPaint?.Invoke(tile);
                }
                cellIndexes.Add(tile.CellIndex);
            }
        }
    }


    private void OnParticleTrigger()
    {
        if (!enabled) return;
        List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside, out var insideData);
        for (int i = 0; i < numInside; i++)
        {
            if (insideData.GetCollider(i, 0).TryGetComponent(out SplashTile tile))
            {
                if (tile.PaintTile(this))
                {
                    OnPaint?.Invoke(tile);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent(out SplashTile tile))
        {
            if (cellIndexes.Contains(tile.CellIndex))
            {
                cellIndexes.Remove(tile.CellIndex);
            }
        }
    }
    
    public void SetColor(PaintColor color)
    {
        paintColor = color;
    }

    private void OnDisable()
    {
        OnPaint = null;
    }

    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
        ps = GetComponentInChildren<ParticleSystem>(true);
    }
}