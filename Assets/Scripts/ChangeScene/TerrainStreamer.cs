using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerrainStreamer : MonoBehaviour
{
    [System.Serializable]
    public class StreamedScene
    {
        public string sceneName;
        public Vector3 center;
        public float loadDistance = 250f;
        public float unloadDistance = 300f;

        [HideInInspector] public bool isLoaded = false;
    }

    public Transform player;
    public List<StreamedScene> scenes = new List<StreamedScene>();

    private void Update()
    {
        foreach (var s in scenes)
        {
            float dist = Vector3.Distance(player.position, s.center);

            // Carga cuando está dentro del radio
            if (!s.isLoaded && dist < s.loadDistance)
            {
                SceneManager.LoadSceneAsync(s.sceneName, LoadSceneMode.Additive);
                s.isLoaded = true;
            }

            // Descarga cuando se aleja lo suficiente
            if (s.isLoaded && dist > s.unloadDistance)
            {
                SceneManager.UnloadSceneAsync(s.sceneName);
                s.isLoaded = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (scenes == null)
            return;

        Gizmos.matrix = Matrix4x4.identity;

        foreach (var s in scenes)
        {
            // Línea en el centro
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(s.center, 1f);

            // Radio de carga - verde
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawWireSphere(s.center, s.loadDistance);

            // Radio de descarga - rojo
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(s.center, s.unloadDistance);
        }
    }
}
