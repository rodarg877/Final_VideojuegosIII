using UnityEngine;
using UnityEditor;

public class ConvertLightsToMixed : EditorWindow
{
    [MenuItem("Tools/Convert Lights to Mixed")]
    public static void ConvertLights()
    {
        // Contador
        int convertedCount = 0;

        // Buscar todas las luces en la escena
        Light[] allLights = FindObjectsOfType<Light>();

        foreach (Light light in allLights)
        {
            if (light.lightmapBakeType == LightmapBakeType.Realtime)
            {
                Undo.RecordObject(light, "Convert Light to Mixed");
                light.lightmapBakeType = LightmapBakeType.Mixed;
                convertedCount++;
            }
        }

        Debug.Log($"✅ Luces convertidas: {convertedCount}");
    }
}
