using UnityEngine;
using System.Collections.Generic;

public class FootstepSystem : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource audioSource;
    public FirstPersonController playerController; // Opcional, solo para chequear Ground

    [Header("Sonidos por Capa de Terreno")]
    public List<AudioClipGroup> terrainLayers; 

    [Header("Sonidos por Tag")]
    public AudioClip[] snowClips;
    public AudioClip[] grassClips;
    public AudioClip[] woodClips;
    public AudioClip[] stoneClips;
    public AudioClip[] defaultClips;

    [Header("Configuración")]
    public LayerMask groundLayer;
    public float rayDistance = 2.5f;
    
    // YA NO NECESITAMOS nextStepTime NI stepInterval
    // El ritmo lo controla el HeadBob del otro script

    [System.Serializable]
    public class AudioClipGroup
    {
        public string layerName;
        public AudioClip[] clips;
    }

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (playerController == null) playerController = GetComponent<FirstPersonController>();
    }

    // Eliminamos el Update(). Ya no calculamos el tiempo aquí.
    
    // Esta función ahora es llamada EXPLICITAMENTE por el FirstPersonController
    public void PlayFootstep()
    {
        // Solo sonamos si estamos tocando el suelo (Doble verificación de seguridad)
        if (playerController != null && !playerController.isGrounded) return;

        DetectSurfaceAndPlay();
    }

    private void DetectSurfaceAndPlay()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance, groundLayer))
        {
            // 1. Chequeo de Terreno
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                int textureIndex = GetDominantTextureIndex(transform.position, terrain);
                if (textureIndex < terrainLayers.Count)
                {
                    PlayOneShotRandom(terrainLayers[textureIndex].clips);
                }
                return;
            }

            // 2. Chequeo de Tags
            switch (hit.collider.tag)
            {
                case "Snow": PlayOneShotRandom(snowClips); break;
                case "Grass": PlayOneShotRandom(grassClips); break;
                case "Wood": PlayOneShotRandom(woodClips); break;
                case "Stone": PlayOneShotRandom(stoneClips); break;
                default: PlayOneShotRandom(defaultClips); break;
            }
        }
    }

    private int GetDominantTextureIndex(Vector3 worldPos, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        float maxMix = 0;
        int maxIndex = 0;
        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            if (splatmapData[0, 0, i] > maxMix)
            {
                maxMix = splatmapData[0, 0, i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    private void PlayOneShotRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        // IMPORTANTE: Evitar solapamiento brusco
        // Si ya está sonando un paso muy fuerte, podemos elegir no interrumpirlo o bajarle volumen
        // Pero con el sistema de HeadBob, el ritmo debería ser perfecto.
        
        int index = Random.Range(0, clips.Length);
        
        // Pitch random para realismo
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.85f, 1.0f);
        
        audioSource.PlayOneShot(clips[index]);
    }
}