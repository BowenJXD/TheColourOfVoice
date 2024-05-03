using System;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public class ColorManager : Singleton<ColorManager>
{
    public SerializableDictionary<PaintColor, Color> colorDict = new SerializableDictionary<PaintColor, Color>
    {
        { PaintColor.Null, Color.clear },
        { PaintColor.Red, Color.red },
        { PaintColor.Yellow, Color.yellow },
        { PaintColor.Green, Color.green },
        { PaintColor.Cyan, Color.cyan },
        { PaintColor.Blue, Color.blue },
        { PaintColor.Magenta, Color.magenta },
        { PaintColor.White, Color.white },
        { PaintColor.Black, Color.black },
    };
    
    public SerializableDictionary<PaintColor, Gradient> gradientDict = new SerializableDictionary<PaintColor, Gradient>
    {
        { PaintColor.Null, new Gradient() },
        { PaintColor.Red, new Gradient() },
        { PaintColor.Yellow, new Gradient() },
        { PaintColor.Green, new Gradient() },
        { PaintColor.Cyan, new Gradient() },
        { PaintColor.Blue, new Gradient() },
        { PaintColor.Magenta, new Gradient() },
        { PaintColor.White, new Gradient() },
        { PaintColor.Black, new Gradient() },
    };

    public Gradient GetGradient(PaintColor color)
    {
        if (gradientDict.ContainsKey(color))
        {
            return gradientDict[color];
        }
        return null;
    }
    
    public Gradient GetGradient(PaintColor color, float shiftH = 0, float shiftS = 0, float shiftV = 0)
    {
        Gradient gradient = gradientDict[color];
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        Color c = GetColor(color);
        Color.RGBToHSV(c, out float h, out float s, out float v);
        h = (h + shiftH) % 1;
        s = Mathf.Clamp01(s + shiftS);
        v = Mathf.Clamp01(v + shiftV);
        colorKeys[0].color = c;
        colorKeys[0].time = 0;
        colorKeys[1].color = Color.HSVToRGB(h, s, v);
        colorKeys[1].time = 1;
        alphaKeys[0].alpha = 1;
        alphaKeys[0].time = 0;
        alphaKeys[1].alpha = 1;
        alphaKeys[1].time = 1;
        gradient.SetKeys(colorKeys, alphaKeys);
        return gradient;
    }
    
    public PaintColor[] rainbowColors = { PaintColor.Red, PaintColor.Yellow, PaintColor.Green, PaintColor.Cyan, PaintColor.Blue, PaintColor.Magenta };
    [ReadOnly] public PaintColor rainbowCurrent = PaintColor.Null;

    public Color GetColor(PaintColor color)
    {
        Color result = Color.black;
        var colorDict = ColorManager.Instance.colorDict;
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

    public PaintColor Next(PaintColor current)
    {
        PaintColor[] values = (PaintColor[])Enum.GetValues(typeof(PaintColor));
        int index = Array.IndexOf(values, current);
        index = (index + 1) % values.Length;
        return values[index];
    }
    
    public PaintColor NextRainbow()
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