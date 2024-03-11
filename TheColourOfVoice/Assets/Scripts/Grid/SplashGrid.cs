using System;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public enum Neighbor
{
    Up,
    Right,
    Down,
    Left,
}

public class SplashGrid : MonoBehaviour
{
    public Vector2Int size;
    
    public Vector2 cellSize;
    
    [Tooltip("Local position of the bottom left corner of the grid.")]
    Vector3 bottomLeftPosition;
    
    public SplashTile tilePrefab;

    Sprite[] tileSprites;
    
    SplashTile[,] tiles;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        var sps = AssetDatabase.LoadAllAssetsAtPath(PathDefines.SplashSprites)
            .OfType<Sprite>();
        tileSprites = new Sprite[16];
        foreach (var sp in sps)
        {
            if (int.TryParse(sp.name.Split("_")[1], out int result))
            {
                tileSprites[result] = sp;
            }
        }

        bottomLeftPosition = - new Vector3(cellSize.x * size.x / 2, cellSize.y * size.y / 2, 0);
        tiles = new SplashTile[size.x, size.y];
        
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                SplashTile tile = Instantiate(tilePrefab, transform);
                tile.CellIndex = new Vector2Int(x, y);
                tile.transform.localScale = cellSize * 1.01f;
                tile.SetLocation(GetCellPosition(tile.CellIndex));
                tiles[x, y] = tile;
                tile.Grid = this;
                tile.Init();
            }
        }
    }

    public void PaintTile(SplashTile tile, MainColor color)
    {
        tile.Color = color;
        tile.Shape = GetTileShape(tile.CellIndex);
        tile.Sprite = tileSprites[tile.Shape];
        tile.IsPainted = true;
        
        ChangeNeighborTileShape(tile.CellIndex);
    }

    public void EraseTile(SplashTile tile)
    {
        tile.Color = MainColor.Null;
        tile.IsPainted = false;
        
        ChangeNeighborTileShape(tile.CellIndex);
    }

    #region Shape
    
    public int GetTileShape(Vector2Int cellIndex)
    {
        bool up = IsTilePainted(cellIndex + Vector2Int.up);
        bool right = IsTilePainted(cellIndex + Vector2Int.right);
        bool down = IsTilePainted(cellIndex + Vector2Int.down);
        bool left = IsTilePainted(cellIndex + Vector2Int.left);
        
        int result = (up ? 1 : 0) << 3 | (right ? 1 : 0) << 2 | (down ? 1 : 0) << 1 | (left ? 1 : 0);
        return result;
    }
    
    public void ChangeNeighborTileShape(Vector2Int cellIndex)
    {
        if (IsTilePainted(cellIndex + Vector2Int.up))
        {
            ChangeTileShape(cellIndex + Vector2Int.up, Neighbor.Down);
        }
        if (IsTilePainted(cellIndex + Vector2Int.right))
        {
            ChangeTileShape(cellIndex + Vector2Int.right, Neighbor.Left);
        }
        if (IsTilePainted(cellIndex + Vector2Int.down))
        {
            ChangeTileShape(cellIndex + Vector2Int.down, Neighbor.Up);
        }
        if (IsTilePainted(cellIndex + Vector2Int.left))
        {
            ChangeTileShape(cellIndex + Vector2Int.left, Neighbor.Right);
        }
    }
    
    public void ChangeTileShape(Vector2Int cellIndex, Neighbor changedNeighbor)
    {
        SplashTile tile = GetTile(cellIndex);
        int mask = 1 << (3 - (int)changedNeighbor);
        int result = tile.Shape ^ mask;
        tile.Shape = result;
        tile.SetSprite(tileSprites[result]);
    }
    
    #endregion
    
    #region Getter and Setter
    
    public SplashTile GetTile(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= size.x || cellIndex.y < 0 || cellIndex.y >= size.y) return null;
        return tiles[cellIndex.x, cellIndex.y];
    }
    
    public bool TryGetTile(Vector2Int cellIndex, out SplashTile tile)
    {
        tile = GetTile(cellIndex);
        return tile != null;
    }
    
    public bool TrySetTile(SplashTile tile, Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= size.x || cellIndex.y < 0 || cellIndex.y >= size.y) return false;
        tiles[cellIndex.x, cellIndex.y] = tile;
        return true;
    }
    
    public bool IsTilePainted(Vector2Int cellIndex)
    {
        return TryGetTile(cellIndex, out SplashTile tile) && tile.IsPainted;
    }
    
    #endregion
    
    public bool TryGetCellIndex(Vector3 position, out Vector2Int cellIndex)
    {
        Vector3 relativePosition = position - (transform.position + bottomLeftPosition);
        int x = Mathf.FloorToInt(relativePosition.x / cellSize.x);
        int y = Mathf.FloorToInt(relativePosition.y / cellSize.y);
        if (x < 0 || x >= size.x || y < 0 || y >= size.y) 
        {
            cellIndex = new Vector2Int(-1, -1);
            return false;
        }
        else
        {
            cellIndex = new Vector2Int(x, y);
            return true;
        }
    }
    
    public Vector3 GetCellPosition(Vector2Int cellIndex)
    {
        return transform.position + bottomLeftPosition + new Vector3(cellIndex.x * cellSize.x, cellIndex.y * cellSize.y, 0);
    }
}