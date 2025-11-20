using UnityEngine;

namespace SkyrimProject.DialogueSystem
{
    [CreateAssetMenu(fileName = "NewGiveQuestionActivable", menuName = "Dialogue/DialogueActivable/GiveQuestion")]
    public class GiveQuestionActivable : DialogueActivable
    {
        [SerializeField] private string questId;

        public override void Activate()
        {
            var questManager = Object.FindFirstObjectByType<QuestSystem.QuestManager>();
            questManager.AcceptQuest(questId);
        }
    }
}
