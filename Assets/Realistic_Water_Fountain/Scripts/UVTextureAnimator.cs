using UnityEngine;

public class UVTextureAnimator : MonoBehaviour
{
    [Header("Sprite Sheet Settings")]
    public int uvAnimationTileX = 24; // Número de columnas
    public int uvAnimationTileY = 1;  // Número de filas
    public float framesPerSecond = 10.0f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calcula el índice del frame actual
        int index = (int)(Time.time * framesPerSecond);
        index = index % (uvAnimationTileX * uvAnimationTileY);

        // Tamaño de cada tile
        Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

        // Índices horizontal y vertical
        int uIndex = index % uvAnimationTileX;
        int vIndex = index / uvAnimationTileX;

        // Invertir el eje Y (porque OpenGL/Unity lo leen desde abajo)
        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        // Aplicar offset y escala
        rend.material.SetTextureOffset("_BaseMap", offset);
        rend.material.SetTextureScale("_BaseMap", size);
    }
}
