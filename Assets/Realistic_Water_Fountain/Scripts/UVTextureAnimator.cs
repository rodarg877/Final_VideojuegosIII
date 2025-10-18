using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVTextureAnimator : MonoBehaviour
{
    [Header("UV Animation Settings")]
    [Tooltip("N�mero de columnas en la hoja de sprites")]
    public int uvAnimationTileX = 24; // columnas
    [Tooltip("N�mero de filas en la hoja de sprites")]
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

        // C�lculo del �ndice de frame actual
        int totalFrames = uvAnimationTileX * uvAnimationTileY;
        int index = (int)(Time.time * framesPerSecond) % totalFrames;

        // Tama�o de cada tile
        Vector2 size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

        // Dividir en �ndice horizontal y vertical
        int uIndex = index % uvAnimationTileX;
        int vIndex = index / uvAnimationTileX;

        // Calcular offset (invertido en eje Y)
        Vector2 offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);

        // Aplicar a la textura del material
        rend.material.SetTextureOffset("_BaseMap", offset);  // URP usa _BaseMap
        rend.material.SetTextureScale("_BaseMap", size);
    }
}

