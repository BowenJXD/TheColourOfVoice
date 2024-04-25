using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
///  A tile in the splash grid.
/// </summary>
public class SplashTile : MonoBehaviour
{
    private SpriteRenderer sp;
    private SpriteRenderer blocking;
    private Transform scaler;

    private int _shape;
    public int Shape
    {
        get => _shape;
        set
        {
            _shape = value;
            Sprite = Grid.GetTileSprite(value);
        }
    }

    private Vector3 originalScale;
    public float aniDuration = 0.3f;
    public Ease ease = Ease.OutBack;
    public bool aniOnSameColor = false;
    
    private PaintColor _color;
    public PaintColor Color
    {
        get => _color;
        set
        {
            if (_color == value && !aniOnSameColor) return;
            if (value != PaintColor.Null)
            {
                scaler.localScale = Vector3.zero;
                scaler.DOScale(originalScale, aniDuration).SetEase(ease);
            }
            else
            {
                scaler.DOScale(Vector3.zero, aniDuration).SetEase(Ease.InBack);
            }
            _color = value;
            sp.DOColor(value.ToColor(), aniDuration).SetEase(ease);
            blocking.DOColor(value.ToColor(), aniDuration).SetEase(ease);
        }
    }

    public Sprite Sprite
    {
        get => blocking.sprite;
        set
        {
            if (blocking) blocking.sprite = value;
        }
    }

    public Vector2Int CellIndex { get; set; }
    
    public SplashGrid Grid { get; set; }
    
    public bool IsPainted { get; set; }
    
    public void Init()
    {
        scaler = transform.GetChild(0);
        var sps = GetComponentsInChildren<SpriteRenderer>();
        blocking = sps[0];
        sp = sps[1];
        gameObject.SetActive(true);
        originalScale = scaler.localScale;
    }

    public void Deinit()
    {
        gameObject.SetActive(false);
    }

    public void SetLocation(Vector3 location)
    {
        transform.position = location;
    }
    
    /// <summary>
    /// Reset after trigger
    /// </summary>
    public Action<Painter> OnPainted;

    public void PaintTile(Painter painter)
    {
        PaintTile(painter.paintColor);
        OnPainted?.Invoke(painter);
        OnPainted = null;
    }
    
    public void PaintTile(PaintColor color)
    {
        if (!IsPainted && color != PaintColor.Null)
        {
            ColorTile(color);
        }
        else if (IsPainted && color != PaintColor.Null)
        {
            Color = color;
        }
        else if (IsPainted && color == PaintColor.Null)
        {
            EraseTile(this);
        }
    }
    
    public void ColorTile(PaintColor color)
    {
        Color = color;
        Shape = Grid.GetTileShape(CellIndex);
        if (!IsPainted) Grid.PaintedCount++;
        IsPainted = true;
        sp.transform.rotation = Grid.GetTileRotation();
        
        Grid.ChangeNeighborTileShape(CellIndex);
    }
    
    public void EraseTile(SplashTile tile)
    {
        Color = PaintColor.Null;
        if (IsPainted) Grid.PaintedCount--;
        IsPainted = false;
        
        Grid.ChangeNeighborTileShape(tile.CellIndex);
    }
}