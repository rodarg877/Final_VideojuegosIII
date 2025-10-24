using UnityEngine;
using UnityEditor;
using System.IO;

public class AsignarTexturas : EditorWindow
{
    [MenuItem("Herramientas/Asignar Texturas a Materiales")]
    public static void Asignar()
    {
        string rutaMateriales = "Assets/Orcs/material herreria"; // Cambiá esto según tu carpeta
        string[] materiales = Directory.GetFiles(rutaMateriales, "*.mat", SearchOption.AllDirectories);

        foreach (string rutaMat in materiales)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(rutaMat);
            if (mat == null) continue;

            string nombreBase = Path.GetFileNameWithoutExtension(rutaMat);
            string[] texturas = Directory.GetFiles(rutaMateriales, nombreBase + "*", SearchOption.AllDirectories);

            foreach (string rutaTex in texturas)
            {
                Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(rutaTex);
                if (tex != null && rutaTex.EndsWith(".png") || rutaTex.EndsWith(".jpg"))
                {
                    mat.mainTexture = tex;
                    EditorUtility.SetDirty(mat);
                    Debug.Log($"✅ Asignada textura '{tex.name}' al material '{mat.name}'");
                    break;
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("🎯 Asignación de texturas completada.");
    }
}