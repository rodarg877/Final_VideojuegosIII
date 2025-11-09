using System;

namespace SkyrimProject.QuestSystem
{
    [Serializable]
    public struct QuestUIInfo
    {
        public string Title;
        public string Description;
        public string StatusText;

        public QuestUIInfo(string title, string description, string statusText)
        {
            Title = title;
            Description = description;
            StatusText = statusText;
        }
    }
}