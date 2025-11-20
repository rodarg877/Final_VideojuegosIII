using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SkyrimProject.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        [Header("Catálogo (assets SO)")]
        [SerializeField] private List<BaseQuest> allQuestsCatalog = new();

        public Action<QuestUIInfo> OnUiInfoChanged;

        private readonly List<IQuestItem> acceptedQuests = new();
        private readonly List<IQuestItem> completedQuests = new();

        public IQuestItem CurrentQuest { get; private set; }

        private void OnDestroy()
        {
            foreach (var quest in allQuestsCatalog)
            {
                if (quest != null) quest.OnStatusChanged -= HandleQuestStatusChanged;
            }

            foreach (var quest in acceptedQuests)
            {
                if (quest != null) quest.OnStatusChanged -= HandleQuestStatusChanged;
            }
        }

        private void Update()
        {
            for (int i = 0; i < acceptedQuests.Count; i++)
            {
                var quest = acceptedQuests[i];
                if (quest == null) continue;

                if (quest.Status is QuestStatus.Accepted)
                {
                    quest.Tick();
                }
            }
        }

        public void AcceptQuest(string Id)
        {
            if (!allQuestsCatalog.Exists((q) => q.Id == Id)) { return; }

            var quest = allQuestsCatalog.Where((q) =>  q.Id == Id ).First();

            if (quest == null) return;

            allQuestsCatalog.Remove(quest);

            quest.OnStatusChanged += HandleQuestStatusChanged;

            quest.StartQuest();

            if (!acceptedQuests.Contains(quest))
            {
                acceptedQuests.Add(quest);
                SetCurrentQuest(quest);
            }
        }

        private void HandleQuestStatusChanged(IQuestItem quest, QuestStatus status, QuestUIInfo uiInfo)
        {
            NotifyUi(uiInfo);

            switch (status)
            {
                case QuestStatus.Accepted:
                    if (CurrentQuest == null)
                    {
                        SetCurrentQuest(quest);
                    }
                break;

                case QuestStatus.Completed:
                    if (!completedQuests.Contains(quest))
                    {
                        completedQuests.Add(quest);
                        acceptedQuests.Remove(quest);
                    }

                    if (CurrentQuest == quest)
                    {
                        CurrentQuest = FindNextAccepted();
                    }
                break;
            }
        }

        private void SetCurrentQuest(IQuestItem quest)
        {
            CurrentQuest = quest;
        }

        private IQuestItem FindNextAccepted()
        {
            foreach (var q in acceptedQuests)
                if (q != null && q.Status is QuestStatus.Accepted)
                    return q;
            return null;
        }

        private void NotifyUi(QuestUIInfo uiInfo) => OnUiInfoChanged?.Invoke(uiInfo);

        public IReadOnlyList<IQuestItem> GetAcceptedQuests() => acceptedQuests.AsReadOnly();
        public IReadOnlyList<IQuestItem> GetCompletedQuests() => completedQuests.AsReadOnly();
    }
}