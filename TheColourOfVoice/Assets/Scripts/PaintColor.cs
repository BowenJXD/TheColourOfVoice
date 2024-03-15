using System;
using UnityEngine;

/// <summary>
/// The main colors used in the project.
/// </summary>
public enum PaintColor
{
    Null,
    Red,
    Green,
    Blue,
    Yellow,
    Cyan,
    Magenta,
    White,
    Black,
}

public static class MainColorExtension
{
    public static Color ToColor(this PaintColor color)
    {
        Color result = Color.black;
        switch (color)
        {
            case PaintColor.Red:
                result = Color.red;
                break;
            case PaintColor.Green:
                result = Color.green;
                break;
            case PaintColor.Blue:
                result = Color.blue;
                break;
            case PaintColor.Yellow:
                result = Color.yellow;
                break;
            case PaintColor.Cyan:
                result = Color.cyan;
                break;
            case PaintColor.Magenta:
                result = Color.magenta;
                break;
            case PaintColor.White:
                result = Color.white;
                break;
            case PaintColor.Black:
                result = Color.black;
                break;
            default :
                result = Color.clear;
                break;
        }
        return result;
    }
}