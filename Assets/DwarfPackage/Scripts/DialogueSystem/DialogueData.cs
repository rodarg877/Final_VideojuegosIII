using System.IO;
using UnityEngine;

namespace SkyrimProject.DialogueSystem
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
    public class DialogueData : ScriptableObject
    {
        [Header("Archivo JSON")]
        [SerializeField] private TextAsset jsonFile;

        [System.Serializable]
        public class DialogueLine
        {
            public string speakerName;
            public string text;
        }

        [System.Serializable]
        private class DialogueWrapper
        {
            public DialogueLine[] lines;
        }

        private DialogueLine[] loadedLines;
        public DialogueLine[] Lines => loadedLines;

        public void LoadFromJson()
        {
            //string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
            //string json = File.ReadAllText(filePath);

            DialogueWrapper wrapper = JsonUtility.FromJson<DialogueWrapper>(jsonFile.text);
            loadedLines = wrapper.lines;
        }
    }
}