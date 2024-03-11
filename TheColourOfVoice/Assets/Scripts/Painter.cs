using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Painter : MonoBehaviour
{
    public MainColor paintColor;
    public Vector2Int CellIndex { get; set; }
    Vector2Int lastCellIndex;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out SplashTile tile))
        {
            if (tile.CellIndex != CellIndex)
            {
                tile.PaintTile(paintColor);
            }
            CellIndex = tile.CellIndex;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out SplashTile tile))
        {
            if (tile.CellIndex != CellIndex)
            {
                tile.PaintTile(paintColor);
            }
            CellIndex = tile.CellIndex;
        }
    }
}