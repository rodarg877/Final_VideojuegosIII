using UnityEngine;
using System;
using SkyrimProject.DialogueSystem;
using System.Linq;
using System.Collections.Generic;

namespace SkyrimProject.DwarvenPackage
{
    public class NPCController : MonoBehaviour, IInteractuable
    {
        [Header("Dialogue")]
        [SerializeField] private DialogueData dialogue;
        [SerializeField] private Transform facePosition;

        [Header("Animation")]
        [SerializeField] private Animator animController;

        [Header("DialogueActivable")]
        [SerializeField] private List<DialogueActivable> dialogueActivables;


        private PathMovement pathMovement;
        private DialogueSystem.DialogueSystem dialogueSystem;

        private Action interactionCompleted;

        private void Start()
        {
            dialogueSystem = FindFirstObjectByType<DialogueSystem.DialogueSystem>();
            pathMovement = GetComponent<PathMovement>();
            SetMovement(true);
        }

        private void StartToDialogue() 
        {
            SetMovement(true);
            dialogueSystem.dialogueCompleted += EndDialogue;
            dialogueSystem.StartDialogue(dialogue, gameObject.transform);
        }

        private void EndDialogue() 
        {
            DialogueActivable();
            interactionCompleted?.Invoke();
            interactionCompleted = null;
            dialogueSystem.dialogueCompleted -= EndDialogue;

            SetMovement(false);
        }

        private void SetMovement(bool movement) 
        {
            if (pathMovement != null)
            {
                pathMovement.SetMovement(movement);
                animController.SetBool("InMovement", movement);
            }
        }

        private void DialogueActivable() 
        {
            if (dialogueActivables.Count() > 0) 
            {
                foreach (var da in dialogueActivables) 
                {
                    da.Activate();
                }
            }
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