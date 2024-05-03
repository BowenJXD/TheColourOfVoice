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
    Yellow,
    Green,
    Cyan,
    Blue,
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