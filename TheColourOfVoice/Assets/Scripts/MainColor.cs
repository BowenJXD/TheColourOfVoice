using System;
using UnityEngine;

public enum MainColor
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
    public static Color ToColor(this MainColor color)
    {
        Color result = Color.black;
        switch (color)
        {
            case MainColor.Red:
                result = Color.red;
                break;
            case MainColor.Green:
                result = Color.green;
                break;
            case MainColor.Blue:
                result = Color.blue;
                break;
            case MainColor.Yellow:
                result = Color.yellow;
                break;
            case MainColor.Cyan:
                result = Color.cyan;
                break;
            case MainColor.Magenta:
                result = Color.magenta;
                break;
            case MainColor.White:
                result = Color.white;
                break;
            case MainColor.Black:
                result = Color.black;
                break;
            default :
                result = Color.clear;
                break;
        }
        return result;
    }
}