using UnityEngine;

public class UVAnimationURP : MonoBehaviour
{
    public int uvAnimationTileX = 24; // Number of columns in your sprite sheet
    public int uvAnimationTileY = 1;  // Number of rows in your sprite sheet
    public float framesPerSecond = 10.0f;

    private Material material;
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    private static readonly int BaseMapST = Shader.PropertyToID("_BaseMap_ST");

    void Start()
    {
        // Get the material from the renderer
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
    }

    void Update()
    {
        if (material == null) return;

        // Calculate index
        int index = (int)(Time.time * framesPerSecond);
        // repeat when exhausting all frames
        index = index % (uvAnimationTileX * uvAnimationTileY);

        // Size of every tile
        Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

        // split into horizontal and vertical index
        int uIndex = index % uvAnimationTileX;
        int vIndex = index / uvAnimationTileX;

        // build offset
        // v coordinate is the bottom of the image in opengl so we need to invert.
        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        // For URP shaders, we need to set both offset and tiling together using _BaseMap_ST
        // _BaseMap_ST.xy = tiling, _BaseMap_ST.zw = offset
        material.SetVector(BaseMapST, new Vector4(size.x, size.y, offset.x, offset.y));

        // Alternative method - set separately (works with most URP shaders)
        // material.SetTextureOffset("_BaseMap", offset);
        // material.SetTextureScale("_BaseMap", size);
    }

    void OnDestroy()
    {
        // Clean up material instance if we created one
        if (material != null && Application.isPlaying)
        {
            Destroy(material);
        }
    }
}