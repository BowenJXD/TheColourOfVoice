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
            if (value != PaintColor.Null/* && value != PaintColor.Black*/)
            {
                scaler.localScale = Vector3.zero;
                scaler.DOScale(originalScale, aniDuration).SetEase(ease);
            }
            else
            {
                scaler.DOScale(Vector3.zero, aniDuration).SetEase(Ease.InBack);
            }
            _color = value;
            var rgb = ColorManager.Instance.GetColor(value);
            sp.DOColor(rgb, aniDuration).SetEase(ease);
            blocking.DOColor(rgb, aniDuration).SetEase(ease);
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
    
    public Func<Painter, bool> OnPainted;

    public bool PaintTile(Painter painter)
    {
        bool result = PaintTile(painter.paintColor);
        if (OnPainted != null)
        {
            if (OnPainted.Invoke(painter))
            {
                OnPainted = null;
            }
        }
        return result;
    }
    
    public bool PaintTile(PaintColor color)
    {
        bool result = false;
        if (Color == PaintColor.Black)
        {
            if (color == PaintColor.White)
            {
                EraseTile(PaintColor.White);
            }
            return false;
        }
        else if (Color == PaintColor.White && color == PaintColor.Black)
        {
            return false;
        }
        
        switch (color)
        {
            case PaintColor.Black when Color != PaintColor.Black:
                EraseTile(PaintColor.Black);
                break;
            case PaintColor.Null when IsPainted:
                EraseTile();
                result = true;
                break;
            case PaintColor.Null:
            case PaintColor.Black:
            case PaintColor.White:
                break;
            default:
            {
                if (!IsPainted)
                {
                    ColorTile(color);
                    result = true;
                }
                else
                {
                    Color = color;
                }

                break;
            }
        }

        return result;
    }
    
    public void ColorTile(PaintColor color)
    {
        Color = color;
        if (!IsPainted) Grid.PaintedCount++;
        IsPainted = true;
        sp.transform.rotation = Grid.GetTileRotation();
        
        Shape = Grid.GetTileShape(CellIndex);
        Grid.ChangeNeighborTileShape(CellIndex);
    }
    
    public void EraseTile(PaintColor color = PaintColor.Null)
    {
        Color = color;
        if (IsPainted) Grid.PaintedCount--;
        if (color != PaintColor.Null) Shape = Grid.GetTileShape(CellIndex, t => t.Color == color);
        IsPainted = false;
        
        Grid.ChangeNeighborTileShape(CellIndex, true);
    }
}