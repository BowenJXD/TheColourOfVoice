using System.IO;
using UnityEngine;

public class SpriteExporter : MonoBehaviour
{
    public Sprite sprite;

    void Start()
    {
        ExportSpriteToPNG(sprite, "Assets/ExportedSprite.png");
    }

    void ExportSpriteToPNG(Sprite sprite, string filePath)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x, 
            (int)sprite.textureRect.y, 
            (int)sprite.textureRect.width, 
            (int)sprite.textureRect.height
        );
        
        texture.SetPixels(pixels);
        texture.Apply();

        byte[] pngData = texture.EncodeToPNG();
        
        if (pngData != null)
        {
            File.WriteAllBytes(filePath, pngData);
            Debug.Log("Sprite exported to: " + filePath);
        }
        else
        {
            Debug.LogError("Failed to convert texture to PNG.");
        }
    }
}