using UnityEngine;

namespace ProjectSurvivor
{
    public class BrushT : MonoBehaviour
    {
        public Camera cam;

        public Color brushColor;

        void Start()
        {
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            if (!Input.GetMouseButton(0))
                return;

            RaycastHit2D hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.CircleCast(ray.origin, 0.01f, ray.direction);
            if (!hit)
                return;

            SpriteRenderer rend = hit.transform.GetComponent<SpriteRenderer>();

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null)
                return;

            Texture2D tex = rend.material.mainTexture as Texture2D;
            if (tex)
            {
                Vector2 pixelUV = rend.GetTextureCoord(hit);
                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;
                
                tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, brushColor);
                tex.Apply();
            }
        }
    }

    public static class BrushUtil
    {
        public static void Draw(this SpriteRenderer sp, Vector2 pixelUV, Color color)
        {
            Texture2D tex = sp.material.mainTexture as Texture2D;
            tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, color);
            tex.Apply();
        }
        
        public static Vector2 GetTextureCoord(this SpriteRenderer sp, RaycastHit2D hit)
        {
            // Calculate the local point on the collider where the ray hit
            Vector2 localHitPoint = hit.transform.InverseTransformPoint(hit.point);

            // Get the sprite renderer and its texture
            Texture2D texture = sp.sprite.texture;

            // Calculate the texture coordinates
            float x = (localHitPoint.x + sp.sprite.bounds.extents.x) / (2 * sp.sprite.bounds.extents.x);
            float y = (localHitPoint.y + sp.sprite.bounds.extents.y) / (2 * sp.sprite.bounds.extents.y);
            return new Vector2(x * texture.width, y * texture.height);
        }
    }
}