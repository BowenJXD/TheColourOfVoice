using System;
using System.Collections;
using UnityEngine;

public class PixelColorGetter : MonoBehaviour
{
    public Camera targetCamera; // The camera from which to get the pixel color

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            StartCoroutine(GetPixelColorCoroutine(screenPos));
        }
    }

    IEnumerator GetPixelColorCoroutine(Vector3 screenPos)
    {
        yield return new WaitForEndOfFrame();
        Color pixelColor = GetPixelColor(screenPos);
        Debug.Log("Pixel color: " + pixelColor);
    }

    private Color GetPixelColor(Vector3 screenPos)
    {
        // Create a new texture to read the pixel color
        Texture2D tex = new Texture2D(1, 1, TextureFormat.RGB24, false);

        // Read the pixel color from the screen position
        Rect rect = new Rect(screenPos.x, Screen.height - screenPos.y, 1, 1);
        tex.ReadPixels(rect, 0, 0);
        tex.Apply();

        // Get the color of the pixel
        Color pixelColor = tex.GetPixel(0, 0);

        // Clean up and return the color
        Destroy(tex);
        return pixelColor;
    }
}