using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>Angle in degrees 0 - 360</returns>
        public static float GetAngleFromVector(Vector2 dir)
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
            float angle1 = GetAngleFromVector(v1);
            float angle2 = GetAngleFromVector(v2);
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
}