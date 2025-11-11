using UnityEngine;
using System.Collections;
using System;
using System.Linq;

namespace SkyrimProject.DialogueSystem
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private KeyCode keyToNext = KeyCode.Space;

        private bool isInDialogue;

        public Action dialogueCompleted;

        public void StartDialogue(DialogueData dialogue, Transform npcTransform)
        {
            if (isInDialogue) return;
            isInDialogue = true;

            // Cargar diálogos desde el JSON asociado
            dialogue.LoadFromJson();

            // Hacer que el NPC mire a la cámara
            Vector3 lookAt = mainCamera.transform.position;
            lookAt.y = npcTransform.position.y;
            npcTransform.LookAt(lookAt);

            StartCoroutine(RunDialogue(dialogue));
        }

        private IEnumerator RunDialogue(DialogueData dialogue)
        {
            foreach (var line in dialogue.Lines)
            {
                dialogueUI.ShowLine(line.speakerName, line.text);
                yield return new WaitUntil(() => Input.GetKeyDown(keyToNext));
                yield return new WaitForEndOfFrame();
            }

            EndDialogue();
        }

        private void EndDialogue()
        {
            dialogueUI.Hide();
            isInDialogue = false;
            dialogueCompleted?.Invoke();
        }
    }
}