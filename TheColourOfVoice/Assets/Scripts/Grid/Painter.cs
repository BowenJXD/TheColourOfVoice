using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

/// <summary>
/// A painter that paints the tiles in the splash grid with a specific color.
/// </summary>
public class Painter : MonoBehaviour
{
    public PaintColor paintColor;
    List<Vector2Int> cellIndexes = new List<Vector2Int>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;
        if (col.TryGetComponent(out SplashTile tile))
        {
            if (!cellIndexes.Contains(tile.CellIndex))
            {
                tile.PaintTile(paintColor);
                cellIndexes.Add(tile.CellIndex);
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
}