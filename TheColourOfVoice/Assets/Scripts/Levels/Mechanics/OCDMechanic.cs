using System;
using System.Collections.Generic;
using UnityEngine;

public class OCDMechanic : LevelMechanic
{
    public float paintInterval = 1f;
    
    public override void Init()
    {
        base.Init();
        PoolManager.Instance.AddGetAction<BulletBase>(OnGetBulletBase);
        FindObjectOfType<PlayerMovement>().fourDirectional = true;
        
        SplashGrid.Instance.doChangeShapeForUnpaintedTiles = true;
        SplashGrid.Instance.OnChangeTileMask += OnChangeTileMask;
        
        /*new LoopTask
        {
            interval = paintInterval,
            loopAction = PaintTiles,
            loop = -1,
        }.Start();*/
    }

    private void OnGetBulletBase(Entity obj)
    {
        if (obj is BulletBase bulletBase)
        {
            bulletBase.fourDirectional = true;
        }
    }

    int[] threeOrMoreSidedMasks = new int[]{7,11,13,14,15};
    /*List<SplashTile> tilesToPaint = new List<SplashTile>(); */
    
    void OnChangeTileMask(Vector2Int pos, int mask)
    {
        // if (mask == 15) SplashGrid.Instance.GetTile(pos).PaintTile(LevelManager.Instance.levelColor);
        if (Array.IndexOf(threeOrMoreSidedMasks, mask) != -1)
        {
            var tile = SplashGrid.Instance.GetTile(pos);
            if (!tile.IsPainted) tile.PaintTile(LevelManager.Instance.levelColor);
            /*tilesToPaint.Add(tile);
            tile.OnPainted += _ => tilesToPaint.Remove(tile);*/
        }
    }

    /*void PaintTiles()
    {
        var temp = new List<SplashTile>(tilesToPaint);
        tilesToPaint.Clear();
        for (int i = 0; i < temp.Count; i++)
        {
            var tile = temp[i];
            if (!tile) continue;
            if (!tile.IsPainted) tile.PaintTile(LevelManager.Instance.levelColor);
        }
    }*/
}