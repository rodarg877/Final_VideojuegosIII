using SkyrimProject.QuestSystem;
using System;
using UnityEngine;

namespace SkyrimProject.QuestSystem
{
    public abstract class BaseQuest : ScriptableObject, IQuestItem
    {
        [Header("Identidad")]
        [SerializeField] private string id = "quest_id_unico";

        [Header("Mensajes UI")]
        [TextArea, SerializeField] protected string initialTitle = "Nueva misión";
        [TextArea, SerializeField] protected string initialDescription = "Nueva misión disponible.";

        [SerializeField] private string availableStatusText = "Disponible";
        [SerializeField] private string acceptedStatusText = "Aceptada";
        [SerializeField] private string completedStatusText = "Completada";

        [Header("Estado inicial")]
        [SerializeField] private QuestStatus initialStatus = QuestStatus.Available;

        // Estado runtime (no se persiste en el asset original)
        [NonSerialized] private QuestStatus status;
        [NonSerialized] private string currentTitle;
        [NonSerialized] private string currentDescription;

        public string Id => id;
        public QuestStatus Status => status;

        public event Action<IQuestItem, QuestStatus, QuestUIInfo> OnStatusChanged;

        // Se llama cuando el SO se crea en memoria
        protected virtual void OnEnable()
        {
            ResetRuntimeState();
        }

        public void ResetRuntimeState()
        {
            status = initialStatus;
            currentTitle = initialTitle;
            currentDescription = initialDescription;
            // Emitimos un primer estado limpio si querés:
            NotifyUi();
        }

        public virtual void StartQuest()
        {
            if (status is QuestStatus.Available)
            {
                ChangeQuestStatus(QuestStatus.Accepted);
                OnStarted();
            }
        }

        public virtual void CompleteQuest()
        {
            if (status is QuestStatus.Accepted)
            {
                ChangeQuestStatus(QuestStatus.Completed);
                OnCompleted();
            }
        }

        public virtual void ChangeQuestStatus(QuestStatus newStatus)
        {
            status = newStatus;

            OnStatusDidChange(newStatus);

            NotifyUi();
        }

        public QuestUIInfo GetUiInfo()
        {
            return new QuestUIInfo(
                currentTitle,
                currentDescription,
                GetStatusText(status)
            );
        }

        public abstract void Tick();

        protected virtual void OnStarted() { }
        protected virtual void OnCompleted() { }
        protected virtual void OnStatusDidChange(QuestStatus newStatus) { }

        /// Cambia solo el Título (y notifica si querés)
        protected void SetUiTitle(string title, bool notify = true)
        {
            if (string.IsNullOrEmpty(title)) return;
            currentTitle = title;
            if (notify) NotifyUi();
        }

        /// Cambia solo la Descripción (y notifica si querés)
        protected void SetUiDescription(string description, bool notify = true)
        {
            if (string.IsNullOrEmpty(description)) return;
            currentDescription = description;
            if (notify) NotifyUi();
        }

        /// Helper centralizado de notificación
        protected void NotifyUi()
        {
            OnStatusChanged?.Invoke(this, status, GetUiInfo());
        }

        /// Mapa de estado → texto visible
        private string GetStatusText(QuestStatus s)
        {
            return s switch
            {
                QuestStatus.Available   => availableStatusText,
                QuestStatus.Accepted    => acceptedStatusText,
                QuestStatus.Completed   => completedStatusText,
                _ => s.ToString()
            };
        }
    }
}