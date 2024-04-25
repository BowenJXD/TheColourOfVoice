using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Takes a text and paints its characters onto the splash grid using <see cref="TextToTile"></see>.
/// </summary>
public class TextPainter : Singleton<TextPainter>
{
    public Dictionary<string, Sequence> paintingSequences = new();
    
    SplashGrid grid;

    public string text;
    public Vector2Int center;
    public PaintColor color;
    public float interval = 0.1f;

    [Button()]
    public void PaintText()
    {
        PaintText(text, center, color, interval);
    }
    
    public void PaintText(string text, Vector2Int center, PaintColor inputColor, float interval, Action<SplashTile> callback = null)
    {
        if (paintingSequences.ContainsKey(text))
        {
            StopPainting(text);
        }
        
        PaintColor color = inputColor == PaintColor.Rainbow ? PaintColorExtension.NextRainbow() : inputColor;

        int totalWidth = 0;
        for (int i = 0; i < text.Length; i++)
        {
            totalWidth += TextToTile.Instance.GetPatternWidth(text[i]) + 1;
        }
        totalWidth -= 1;
        int x = center.x - totalWidth / 2;
        
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < text.Length; i++)
        {
            Vector2Int charCenter = new Vector2Int(x, center.y);
            var i1 = i;
            sequence.AppendCallback(() =>
            {
                PaintChar(text[i1], charCenter, color, callback);
                color = inputColor == PaintColor.Rainbow ? PaintColorExtension.NextRainbow() : inputColor;
            });
            sequence.AppendInterval(interval);
            x += TextToTile.Instance.GetPatternWidth(text[i]) + 1;
        }
        paintingSequences.Add(text, sequence);
        sequence.OnComplete(() => paintingSequences.Remove(text));
        sequence.Play();
    }
    
    public void StopPainting(string text)
    {
        if (paintingSequences.TryGetValue(text, out var sequence))
        {
            sequence.Kill();
            paintingSequences.Remove(text);
        }
    }

    void PaintChar(char character, Vector2Int start, PaintColor color, Action<SplashTile> callback = null)
    {
        var pattern = TextToTile.Instance.GetPattern(character);
        if (!grid) grid = SplashGrid.Instance;
        foreach (var index in pattern)
        {
            if (grid.TryGetTile(start + index, out SplashTile tile))
            {
                tile.PaintTile(color);
                callback?.Invoke(tile);
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach (var sequence in paintingSequences.Values)
        {
            sequence.Kill();
        }
    }
}