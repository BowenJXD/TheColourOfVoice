using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// 各种与随机相关的工具集合
/// </summary>
public static class RandomUtil
{
    public static Random rand = new Random();

    public static int Rand(int max)
    {
        return rand.Next(Mathf.Abs(max));
    }

    public static bool RandBool()
    {
        return rand.Next(2) == 0;
    }

    public static int RandRange(int a, int b)
    {
        int min = Mathf.Min(a, b);
        int max = Mathf.Max(a, b);
        return rand.Next(min, max);
    }

    /// <summary>
    /// Returns a boolean value with a chance of <paramref name="chance"/> to be true
    /// </summary>
    /// <param name="chance"></param>
    /// <returns></returns>
    public static bool Rand100(float chance)
    {
        return rand.Next(100) < chance;
    }

    /// <summary>
    ///  Returns the number of times a chance of <paramref name="chance"/> is true in <paramref name="count"/> tries
    /// </summary>
    /// <param name="chance"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static int Rand100(float chance, int count)
    {
        int result = 0;
        for (int i = 0; i < count; i++)
        {
            if (Rand100(chance))
            {
                result++;
            }
        }

        return result;
    }

    /// <summary>
    /// Returns a random unit vector
    /// </summary>
    /// <returns></returns>
    public static Vector2 RandV2()
    {
        float angle = (float)rand.NextDouble() * 360;
        return Util.GetVectorFromAngle(angle);
    }

    public static int GetRandomWeightedIndex(this List<int> list)
    {
        int totalWeight = list.Sum();
        if (totalWeight == 0)
        {
            Debug.LogWarning("The total weight is 0.");
            return 0;
        }
        int randomValue = rand.Next(totalWeight);
        int result = 0;
        
        for (int i = 0; i < list.Count; i++)
        {
            randomValue -= list[i];
            if (randomValue <= 0)
            {
                result = i;
                break;
            }
        }
        
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != 0)
            {
                return i;
            }
        }

        // This should not happen if the total weight is correct
        throw new InvalidOperationException("Unable to retrieve a random key.");
    }

    /// <summary>
    /// Get random items from list without repeating
    /// </summary>
    /// <param name="list"></param>
    /// <param name="count"></param>
    /// <param name="getWeight"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> GetRandomItems<T>(this List<T> list, int count, Func<T, int> getWeight = null)
    {
        if (getWeight == null)
        {
            getWeight = (item) => 1;
        }

        List<T> listCache = new List<T>(list);
        count = Mathf.Clamp(count, 0, listCache.Count);
        List<T> result = new List<T>();
        List<int> weights = listCache.Select(getWeight).ToList();
        for (int i = count - 1; i >= 0; i--)
        {
            int randomIndex = GetRandomWeightedIndex(weights);
            result.Add(listCache[randomIndex]);
            weights.RemoveAt(randomIndex);
            listCache.RemoveAt(randomIndex);
        }

        return result;
    }
    
    public static T GetRandomItem<T>(this List<T> list)
    {
        return list.GetRandomItems(1).FirstOrDefault();
    }

    public static T GetRandomItems<T>(this Dictionary<T, int> dict)
    {
        List<T> list = new List<T>();
        List<int> weights = new List<int>();
        foreach (var pair in dict)
        {
            list.Add(pair.Key);
            weights.Add(pair.Value);
        }

        int randomIndex = GetRandomWeightedIndex(weights);
        return list[randomIndex];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A random relative position from the camera</returns>
    public static Vector3 GetRandomScreenPosition(float marginPercentage = 0.1f)
    {
        Camera mainCamera = Camera.main;
        float cameraHeight = 2 * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        marginPercentage = Mathf.Clamp(1 - marginPercentage, 0, 1);

        float randomX = ((float)rand.NextDouble() * cameraWidth - cameraWidth / 2) * marginPercentage;
        float randomY = ((float)rand.NextDouble() * cameraHeight - cameraHeight / 2) * marginPercentage;

        Vector3 randomWorldPosition = new Vector3(randomX, randomY, 10f); // 10f is the distance from the camera
        // Vector3 randomWorldPosition = mainCamera.ScreenToWorldPoint(randomScreenPosition);

        return randomWorldPosition;
    }

    public static Vector2 GetRandomNormalizedVector()
    {
        float randomAngle = rand.Next(360);
        Vector2 randomDirection = Util.GetVectorFromAngle(randomAngle);

        return randomDirection;
    }
}