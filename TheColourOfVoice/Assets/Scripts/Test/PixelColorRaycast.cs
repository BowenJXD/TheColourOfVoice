using UnityEngine;

public class PixelColorRaycast : MonoBehaviour
{
    public Camera mainCamera;
    public MeshRenderer meshRenderer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Get the UV coordinates of the hit point
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider != null && meshCollider.sharedMesh != null)
                {
                    Vector2 uv = hit.textureCoord;

                    // Get the texture of the mesh
                    Texture2D texture = meshRenderer.material.mainTexture as Texture2D;

                    // Sample the texture at the UV coordinates
                    Color pixelColor = texture.GetPixelBilinear(uv.x, uv.y);

                    Debug.Log("Pixel color: " + pixelColor);
                }
            }
        }
    }
}