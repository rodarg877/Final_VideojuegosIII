using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVTextureAnimator : MonoBehaviour
{
    [Header("UV Animation Settings")]
    [Tooltip("Número de columnas en la hoja de sprites")]
    public int uvAnimationTileX = 24; // columnas
    [Tooltip("Número de filas en la hoja de sprites")]
    public int uvAnimationTileY = 1;  // filas
    [Tooltip("Cuadros por segundo")]
    public float framesPerSecond = 10.0f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (rend == null || rend.material == null || rend.material.mainTexture == null)
            return;

        // Cálculo del índice de frame actual
        int totalFrames = uvAnimationTileX * uvAnimationTileY;
        int index = (int)(Time.time * framesPerSecond) % totalFrames;

        // Tamaño de cada tile
        Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

        // Dividir en índice horizontal y vertical
        int uIndex = index % uvAnimationTileX;
        int vIndex = index / uvAnimationTileX;

        // Calcular offset (invertido en eje Y)
        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        // Aplicar a la textura del material
        rend.material.SetTextureOffset("_BaseMap", offset);  // URP usa _BaseMap
        rend.material.SetTextureScale("_BaseMap", size);
    }
}

