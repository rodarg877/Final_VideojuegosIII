using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Tus 4 Canales de Ambiente")]
    public AudioSource sourceHumanos;
    public AudioSource sourceElfos;
    public AudioSource sourceOrcos;
    public AudioSource sourceEnanos;
    
    private AudioSource[] allSources;
    public float maxVolume = 0.5f;
    public float fadeDuration = 2.0f; // Segundos que tarda en cambiar

    void Awake()
    {
        // PATRÓN SINGLETON PERSISTENTE
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // <--- ¡LA CLAVE! Esto evita que se borre al cambiar de escena
            
            // Inicializamos el array
            allSources = new AudioSource[] { sourceHumanos, sourceElfos, sourceOrcos, sourceEnanos };
        }
        else
        {
            // Si ya existe un DJ (porque venimos de otra escena), borramos al impostor nuevo
            Destroy(gameObject);
        }
    }

    // Llamaremos a esto al iniciar cada escena nueva
    public void CambiarBioma(int nuevoBiomaID)
    {
        StopAllCoroutines(); // Detenemos fades anteriores si los hubiera
        StartCoroutine(FadeToBiome(nuevoBiomaID));
    }

    // La magia del Crossfade (Corrutina)
    private System.Collections.IEnumerator FadeToBiome(int targetID)
    {
        float timer = 0f;
        float[] startVolumes = new float[allSources.Length];

        // Guardamos los volúmenes actuales
        for (int i = 0; i < allSources.Length; i++)
        {
            startVolumes[i] = allSources[i].volume;
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            for (int i = 0; i < allSources.Length; i++)
            {
                if (i == targetID)
                {
                    // Subir volumen al bioma nuevo
                    allSources[i].volume = Mathf.Lerp(startVolumes[i], maxVolume, t);
                }
                else
                {
                    // Bajar volumen a los demás
                    allSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, t);
                }
            }
            yield return null;
        }

        // Aseguramos valores finales limpios
        for (int i = 0; i < allSources.Length; i++)
        {
            allSources[i].volume = (i == targetID) ? 1f : 0f;
        }
    }
}