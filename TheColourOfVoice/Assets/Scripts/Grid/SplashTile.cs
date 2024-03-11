using System;
using UnityEngine;

public class SplashTile : MonoBehaviour
{
    private SpriteRenderer sp;

    public int Shape { get; set; }

    private MainColor _color;
    public MainColor Color
    {
        get => _color;
        set {
            _color = value;
            sp.color = value.ToColor();
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
        sp = GetComponent<SpriteRenderer>();
        gameObject.SetActive(true);
    }

    public void Deinit()
    {
        gameObject.SetActive(false);
    }
    
    public void SetSprite(Sprite sprite)
    {
        if (sp)
        {
            sp.sprite = sprite;
        }
    }

    public void SetLocation(Vector3 location)
    {
        transform.position = location;
    }

    public void PaintTile(MainColor color)
    {
        if (!IsPainted && color != MainColor.Null)
        {
            Grid.PaintTile(this, color);
        }
        else if (IsPainted && color != MainColor.Null)
        {
            Color = color;
        }
        else if (IsPainted && color == MainColor.Null)
        {
            Grid.EraseTile(this);
        }
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