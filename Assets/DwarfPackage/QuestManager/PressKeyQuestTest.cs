using SkyrimProject.QuestSystem;
using UnityEngine;

namespace SkyrimProject.QuestSystem
{
    [CreateAssetMenu(fileName = "PressKeyQuest", menuName = "Quests/Press Key")]
    public class PressKeyQuest : BaseQuest
    {
        [Header("Condición")]
        [SerializeField] private KeyCode keyToPress = KeyCode.E;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetUiTitle(initialTitle);
        }

        protected override void OnStarted()
        {
            SetUiDescription($"{initialDescription} (Tecla: {keyToPress})");
        }

        public override void Tick()
        {
            if (Status is QuestStatus.Accepted)
            {
                if (Input.GetKeyDown(keyToPress))
                {
                    CompleteQuest();
                }
            }
        }
    }
}