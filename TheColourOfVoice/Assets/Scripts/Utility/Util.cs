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
    public static Vector2 FindNearestV2WithAngle(Vector2 target, List<Vector2> vector2s)
    {
        Vector2 result = Vector2.zero;
        float minAngle = float.MaxValue;
        foreach (var vector2 in vector2s)
        {
            float angle = Vector2.Angle(target, vector2);
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
    
    public static Vector2 GetNearestFourDirection(Vector2 direction)
    {
        float[] angles = new float[]{0, 90, 180, 270};
        Vector2 result = FindNearestV2WithAngle(direction,
            new List<Vector2>(System.Array.ConvertAll(angles, GetVectorFromAngle)));
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
}