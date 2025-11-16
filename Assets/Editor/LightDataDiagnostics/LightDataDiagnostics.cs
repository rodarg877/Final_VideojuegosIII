#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text;

/// <summary>
/// Editor window to scan scenes for UniversalAdditionalLightData, Light and GameObjects.
/// Use Tools > Diagnostics > Light Data Diagnostics to open.
/// </summary>
public class LightDataDiagnostics : EditorWindow
{
 Vector2 _scroll;
 int _maxEntries =50;
 string _lastReport;

 [MenuItem("Tools/Diagnostics/Light Data Diagnostics")]
 public static void ShowWindow() => GetWindow<LightDataDiagnostics>("Light Data Diagnostics");

 void OnGUI()
 {
 EditorGUILayout.LabelField("Scan scenes for UniversalAdditionalLightData, Light and GameObjects", EditorStyles.boldLabel);
 EditorGUILayout.Space();

 _maxEntries = EditorGUILayout.IntField("Max entries to show", _maxEntries);

 if (GUILayout.Button("Scan Current Loaded Scenes"))
 Scan();

 EditorGUILayout.Space();

 _scroll = EditorGUILayout.BeginScrollView(_scroll);
 if (!string.IsNullOrEmpty(_lastReport))
 EditorGUILayout.TextArea(_lastReport, GUILayout.ExpandHeight(true));
 EditorGUILayout.EndScrollView();
 }

 void Scan()
 {
 var sb = new StringBuilder();

 // Find all UniversalAdditionalLightData (include inactive and disabled)
 var uads = Resources.FindObjectsOfTypeAll<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>();
 var uadsScene = uads.Where(u => IsSceneObject(u.gameObject)).ToArray();
 sb.AppendLine($"UniversalAdditionalLightData: total={uads.Length}, in scenes={uadsScene.Length}");

 for (int i =0; i < Mathf.Min(uadsScene.Length, _maxEntries); i++)
 {
 var u = uadsScene[i];
 sb.AppendLine($" [{i}] {GetGameObjectPath(u.gameObject)} active={u.gameObject.activeInHierarchy}");
 }

 // Lights (scene)
 var lightsAll = Resources.FindObjectsOfTypeAll<Light>();
 var lights = lightsAll.Where(l => IsSceneObject(l.gameObject)).ToArray();
 sb.AppendLine($"Light (in scenes): total={lights.Length}");
 for (int i =0; i < Mathf.Min(lights.Length, _maxEntries); i++)
 {
 var l = lights[i];
 sb.AppendLine($" [{i}] {GetGameObjectPath(l.gameObject)} active={l.gameObject.activeInHierarchy} type={l.type}");
 }

 // Approximate GameObject count in loaded scenes
 int goCount =0;
 for (int s =0; s < UnityEngine.SceneManagement.SceneManager.sceneCount; s++)
 {
 var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(s);
 if (!scene.isLoaded) continue;
 var roots = scene.GetRootGameObjects();
 foreach (var r in roots)
 goCount += r.GetComponentsInChildren<Transform>(true).Length;
 }
 sb.AppendLine($"Approx GameObject count in loaded scenes: {goCount}");

 _lastReport = sb.ToString();
 Debug.Log(_lastReport);
 }

 static bool IsSceneObject(GameObject go)
 {
 // Scene objects have a valid scene
 return go != null && go.scene.IsValid();
 }

 static string GetGameObjectPath(GameObject go)
 {
 if (go == null) return "<null>";
 string path = go.name;
 var t = go.transform;
 while (t.parent != null)
 {
 t = t.parent;
 path = t.name + "/" + path;
 }
 return path + $" (scene:{go.scene.name})";
 }
}
#endif
