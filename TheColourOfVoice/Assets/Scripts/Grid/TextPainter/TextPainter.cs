using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Takes a text and paints its characters onto the splash grid using <see cref="TextToTile"></see>.
/// TODO: Add support for looping in different colors.
/// </summary>
public class TextPainter : Singleton<TextPainter>
{
    public Dictionary<string, Sequence> paintingSequences = new();
    
    SplashGrid grid;

    [Button()]
    public void PaintText(string text, Vector2Int center, PaintColor color, float interval)
    {
        if (paintingSequences.ContainsKey(text))
        {
            StopPainting(text);
        }
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < text.Length; i++)
        {
            int x = center.x + (-text.Length / 2 + i) * (TextToTile.patternWidth + 1) + (TextToTile.patternWidth / 2 + 1);
            Vector2Int charCenter = new Vector2Int(x, center.y);
            var i1 = i;
            sequence.AppendCallback(() => PaintChar(text[i1], charCenter, color));
            sequence.AppendInterval(interval);
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

    void PaintChar(char character, Vector2Int center, PaintColor color)
    {
        var pattern = TextToTile.Instance.GetPattern(character);
        if (!grid) grid = SplashGrid.Instance;
        foreach (var index in pattern)
        {
            if (grid.TryGetTile(center + index, out SplashTile tile))
            {
                tile.PaintTile(color);
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