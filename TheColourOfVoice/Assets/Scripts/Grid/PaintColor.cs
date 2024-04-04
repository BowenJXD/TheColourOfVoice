using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The main colors used in the project.
/// </summary>
public enum PaintColor
{
    /// <summary>
    /// Painting null will erase the tile.
    /// </summary>
    Null,
    Red,
    Green,
    Blue,
    Yellow,
    Cyan,
    Magenta,
    White,
    Black,
    /// <summary>
    /// Painting rainbow will paint the tile with the current rainbow color.
    /// Use PaintColor.NextRainbow() to change the current rainbow color.
    /// </summary>
    Rainbow,
    Random,
}

public static class PaintColorExtension
{
    public static Dictionary<PaintColor, Color> colorDict = new Dictionary<PaintColor, Color>
    {
        { PaintColor.Null, Color.clear },
        { PaintColor.Red, Color.red },
        { PaintColor.Green, Color.green },
        { PaintColor.Blue, Color.blue },
        { PaintColor.Yellow, Color.yellow },
        { PaintColor.Cyan, Color.cyan },
        { PaintColor.Magenta, Color.magenta },
        { PaintColor.White, Color.white },
        { PaintColor.Black, Color.black },
    };
    
    public static PaintColor[] rainbowColors = { PaintColor.Red, PaintColor.Yellow, PaintColor.Green, PaintColor.Cyan, PaintColor.Blue, PaintColor.Magenta };
    public static PaintColor rainbowCurrent = PaintColor.Null;

    public static Color ToColor(this PaintColor color)
    {
        Color result = Color.black;
        if (colorDict.ContainsKey(color))
        {
            result = colorDict[color];
        }
        else if (color == PaintColor.Rainbow)
        {
            result = colorDict[rainbowCurrent];
        }
        return result;
    }

    public static PaintColor Next(this PaintColor current)
    {
        PaintColor[] values = (PaintColor[])Enum.GetValues(typeof(PaintColor));
        int index = Array.IndexOf(values, current);
        index = (index + 1) % values.Length;
        return values[index];
    }
    
    public static PaintColor NextRainbow()
    {
        if (!rainbowColors.Contains(rainbowCurrent))
        {
            rainbowCurrent = PaintColor.Red;
        }
        else
        {
            int index = Array.IndexOf(rainbowColors, rainbowCurrent);
            index = (index + 1) % rainbowColors.Length;
            rainbowCurrent = rainbowColors[index];
        }
        return rainbowCurrent;
    }
}