using System;
using UnityEngine;

namespace SkyrimProject.QuestSystem
{
    public enum QuestStatus
    {
        Available,
        Accepted,
        Completed
    }

    public interface IQuestItem
    {
        string Id { get; }
        QuestStatus Status { get; }

        QuestUIInfo GetUiInfo();

        event Action<IQuestItem, QuestStatus, QuestUIInfo> OnStatusChanged;

        void StartQuest();
        void CompleteQuest();
        void ChangeQuestStatus(QuestStatus newStatus);
        void Tick();
    }
}