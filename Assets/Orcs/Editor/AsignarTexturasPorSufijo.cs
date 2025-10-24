using UnityEngine;
using UnityEditor;
using System.IO;

public class AsignarTexturasPorSufijo : EditorWindow
{
    [MenuItem("Herramientas/Asignar Texturas por Sufijo")]//"C:\Users\Victor\Documents\GitHub\Final_VideojuegosIII\Assets\Orcs\Character\WichDrMaterial\textures"
    public static void Asignar()
    {
        string carpeta = "Assets/Orcs/Character/WichDrMaterial/textures"; // Ajustá según tu estructura
        string[] materiales = Directory.GetFiles(carpeta, "*.mat", SearchOption.AllDirectories);

        foreach (string rutaMat in materiales)
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(rutaMat);
            if (mat == null) continue;

            string nombreBase = Path.GetFileNameWithoutExtension(rutaMat);

            Asignar(mat, carpeta, nombreBase + "_Base_Color", "_MainTex");
            Asignar(mat, carpeta, nombreBase + "_Metallic", "_MetallicGlossMap");
            Asignar(mat, carpeta, nombreBase + "_Normal_OpenGL", "_BumpMap", true);
            Asignar(mat, carpeta, nombreBase + "_Mixed_AO", "_OcclusionMap");
            //Asignar(mat, carpeta, nombreBase + "_Opacity", "_MainTex"); // Requiere shader con transparencia
            //Asignar(mat, carpeta, nombreBase + "_Roughness", "_MetallicGlossMap"); // Unity usa canal alpha para smoothness

            EditorUtility.SetDirty(mat);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("🧩 Asignación por sufijo completada.");
    }

    static void Asignar(Material mat, string carpeta, string nombreTextura, string propiedad, bool convertirNormal = false)
    {
        string[] texturas = Directory.GetFiles(carpeta, nombreTextura + "*", SearchOption.AllDirectories);
        foreach (string rutaTex in texturas)
        {
            if (rutaTex.EndsWith(".png") || rutaTex.EndsWith(".jpg"))
            {
                Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(rutaTex);
                if (tex != null)
                {
                    if (convertirNormal)
                    {
                        TextureImporter ti = AssetImporter.GetAtPath(rutaTex) as TextureImporter;
                        if (ti != null)
                        {
                            ti.textureType = TextureImporterType.NormalMap;
                            ti.SaveAndReimport();
                        }
                    }

                    mat.SetTexture(propiedad, tex);
                    Debug.Log($"✅ '{tex.name}' asignada a '{mat.name}' → {propiedad}");
                    return;
                }
            }
        }
    }
}