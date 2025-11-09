using UnityEngine;
using TMPro;

namespace SkyrimProject.DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private TMP_Text dialogueText;

        public void ShowLine(string speaker, string text)
        {
            panel.SetActive(true);
            speakerText.text = speaker;
            dialogueText.text = text;
        }

        public void Hide()
        {
            panel.SetActive(false);
        }
    }
}