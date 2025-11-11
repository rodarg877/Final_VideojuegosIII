using UnityEngine;
using System;
using SkyrimProject.DialogueSystem;

namespace SkyrimProject.DwarvenPackage
{
    public class NPCController : MonoBehaviour, IInteractuable
    {
        [Header("Dialogue")]
        [SerializeField] private DialogueData dialogue;
        [SerializeField] private Transform facePosition;

        [Header("Animation")]
        [SerializeField] private Animator animController;

        private PathMovement pathMovement;
        private DialogueSystem.DialogueSystem dialogueSystem;

        private Action interactionCompleted;

        private void Start()
        {
            dialogueSystem = FindFirstObjectByType<DialogueSystem.DialogueSystem>();
            pathMovement = GetComponent<PathMovement>();
            animController.SetBool("InMovement", true);
        }

        public void StartToDialogue() 
        {
            pathMovement.SetMovement(false);
            animController.SetBool("InMovement", false);
            dialogueSystem.dialogueCompleted += EndDialogue;
            dialogueSystem.StartDialogue(dialogue, gameObject.transform);
        }

        public void EndDialogue() 
        {
            interactionCompleted?.Invoke();
            interactionCompleted = null;
            dialogueSystem.dialogueCompleted -= EndDialogue;
            pathMovement.SetMovement(true);
            animController.SetBool("InMovement", true);
        }

        public void OnInteraction()
        {
            StartToDialogue();
        }

        public void OffInteraction(Action completion)
        {
            interactionCompleted += completion;
        }

        public Transform PositionToFace() 
        {
            return facePosition;
        }
    }
}