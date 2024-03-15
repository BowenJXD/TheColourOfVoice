using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
///  A tile in the splash grid.
/// </summary>
public class SplashTile : MonoBehaviour
{
    private SpriteRenderer sp;

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
            _color = value;
            if (value != PaintColor.Null)
            {
                sp.transform.localScale = Vector3.zero;
                sp.transform.DOScale(originalScale, aniDuration).SetEase(ease);
            }
            else
            {
                sp.transform.DOScale(Vector3.zero, aniDuration).SetEase(Ease.InBack);
            }
            sp.DOColor(value.ToColor(), aniDuration).SetEase(ease);
        }
    }

    public Sprite Sprite
    {
        get => sp.sprite;
        set
        {
            if (sp) sp.sprite = value;
        }
    }

    public Vector2Int CellIndex { get; set; }
    
    public SplashGrid Grid { get; set; }
    
    public bool IsPainted { get; set; }
    
    public void Init()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        gameObject.SetActive(true);
        originalScale = sp.transform.localScale;
    }

    public void Deinit()
    {
        gameObject.SetActive(false);
    }

    public void SetLocation(Vector3 location)
    {
        transform.position = location;
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
        IsPainted = true;
        
        Grid.ChangeNeighborTileShape(CellIndex);
    }
    
    public void EraseTile(SplashTile tile)
    {
        Color = PaintColor.Null;
        IsPainted = false;
        
        Grid.ChangeNeighborTileShape(tile.CellIndex);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Painter painter))
        {
            if (painter.CellIndex != CellIndex)
            {
                PaintTile(painter.paintColor);
            }
            painter.CellIndex = CellIndex;
        }
    }
}