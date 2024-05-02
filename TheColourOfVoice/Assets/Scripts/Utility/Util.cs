using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class Util
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dir"></param>
    /// <returns>Angle in degrees 0 - 360</returns>
    public static float GetAngle(this Vector2 dir)
    {
        return Vector2.SignedAngle(Vector2.right, dir);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle">Angle in degrees (0 - 360)</param>
    /// <returns>Normalized vector derived from angle (from positive x axis)</returns>
    public static Vector2 GetVectorFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public static Vector2 AddAnglesInV2(Vector2 v1, Vector2 v2)
    {
        float angle1 = v1.GetAngle();
        float angle2 = v2.GetAngle();
        float angle = angle1 + angle2;
        return GetVectorFromAngle(angle);
    }

    /// <summary>
    /// Find the 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="vector2s"></param>
    /// <returns></returns>
    public static Vector2 FindNearestV2WithAngle(Vector2 target, Vector2 source, List<Vector2> vector2s)
    {
        Vector2 result = Vector2.zero;
        float minAngle = float.MaxValue;
        foreach (var vector2 in vector2s)
        {
            float angle = Vector2.Angle(target - source, vector2 - source);
            if (angle < minAngle)
            {
                minAngle = angle;
                result = vector2;
            }
        }

        return result;
    }

    public static float[] GenerateAngles(int count, float step)
    {
        float[] result = new float[count];
        float start = step * (count - 1) / 2;
        for (int i = 0; i < count; i++)
        {
            result[i] = start - i * step;
        }

        return result;
    }

    public static float GetScale(this Vector3 vector)
    {
        // use average
        return (vector.x + vector.y + vector.z) / 3;
    }

    /// <summary>
    /// flash every 0.5s
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="flashColor"></param>
    /// <param name="flashDuration"></param>
    public static void FlashSprite(this SpriteRenderer spriteRenderer, Color flashColor, float flashDuration)
    {
        // Create a sequence of tweens to flash the sprite
        DG.Tweening.Sequence flashSequence = DOTween.Sequence();
        const float flashInterval = 0.5f;

        int loopCount = Mathf.CeilToInt(flashDuration / flashInterval * 2);
        int intervalCount = 2 * loopCount + 1;
        float remainder = flashDuration % flashInterval;
        float divisor = flashDuration - remainder;

        // wait for a short time before starting the flash
        flashSequence.AppendInterval(remainder);

        for (int i = 0; i < loopCount; i++)
        {
            flashSequence.Append(spriteRenderer.DOColor(flashColor, divisor / intervalCount));
            // exclude the last loop
            if (i < loopCount - 1)
            {
                flashSequence.Append(spriteRenderer.DOColor(Color.white, divisor / intervalCount));
            }
            else
            {
                flashSequence.Append(spriteRenderer.DOColor(Color.white, 0));
            }
        }

        flashSequence.Play();
    }
    
    public static Component TryGetComponentInChildren<T>(this Component gameObject, out T component, bool includeInactive = true) where T : Component
    {
        component = gameObject.GetComponentInChildren<T>(includeInactive);
        return component;
    }
    
    public static Gradient GetGradient(this Color inputColor, float hueVariance, float saturationVariance, float valueVariance)
    {
        // Convert input color from RGB to HSV
        Color.RGBToHSV(inputColor, out float h, out float s, out float v);
        
        // Create a new Gradient
        Gradient gradient = new Gradient();

        // Calculate the hue, saturation, and value ranges
        float hueMin = Mathf.Clamp01(h - hueVariance);
        float hueMax = Mathf.Clamp01(h + hueVariance);
        float saturationMin = Mathf.Clamp01(s - saturationVariance);
        float saturationMax = Mathf.Clamp01(s + saturationVariance);
        float valueMin = Mathf.Clamp01(v - valueVariance);
        float valueMax = Mathf.Clamp01(v + valueVariance);

        // Generate two colors with adjusted hue, saturation, and value
        Color colorMin = Color.HSVToRGB(hueMin, saturationMin, valueMin);
        Color colorMax = Color.HSVToRGB(hueMax, saturationMax, valueMax);

        // Define the color keys for the gradient
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(colorMin, 0f); // Start of the gradient
        colorKeys[1] = new GradientColorKey(colorMax, 1f); // End of the gradient

        // Define the alpha keys for the gradient (both fully opaque)
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 0f); // Start alpha
        alphaKeys[1] = new GradientAlphaKey(1f, 1f); // End alpha

        // Assign the color and alpha keys to the gradient
        gradient.SetKeys(colorKeys, alphaKeys);

        // Return the generated gradient
        return gradient;
    }
}