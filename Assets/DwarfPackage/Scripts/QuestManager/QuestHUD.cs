using UnityEngine;
using TMPro;

namespace SkyrimProject.QuestSystem
{
    public class QuestHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QuestManager questManager;

        [Header("UI")]
        [SerializeField] private TMP_Text titleText;         // Título de la misión
        [SerializeField] private TMP_Text descriptionText;   // Descripción
        [SerializeField] private TMP_Text statusText;        // Estado legible (Disponible / En progreso / etc.)

        private void OnEnable()
        {
            if (questManager != null)
                questManager.OnUiInfoChanged += UpdateQuestHud;

            // Sincroniza si ya hay una misión activa al habilitar el HUD
            if (questManager != null && questManager.CurrentQuest != null)
                UpdateQuestHud(questManager.CurrentQuest.GetUiInfo());
        }

        private void OnDisable()
        {
            if (questManager != null)
                questManager.OnUiInfoChanged -= UpdateQuestHud;
        }

        public void UpdateQuestHud(QuestUIInfo ui)
        {
            if (titleText != null) titleText.text = ui.Title ?? string.Empty;
            if (descriptionText != null) descriptionText.text = ui.Description ?? string.Empty;
            if (statusText != null) statusText.text = ui.StatusText ?? string.Empty;
        }

        public void Clear()
        {
            if (titleText != null) titleText.text = string.Empty;
            if (descriptionText != null) descriptionText.text = string.Empty;
            if (statusText != null) statusText.text = string.Empty;
        }
    }
}