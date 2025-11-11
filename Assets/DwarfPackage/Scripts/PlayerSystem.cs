using SkyrimProject.DialogueSystem;
using SkyrimProject.DwarvenPackage;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkyrimProject.PlayerSystem
{
    public class PlayerSystem : MonoBehaviour
    {
        [Header("Main Controller")]
        [SerializeField] private FirstPersonController firstPersonController;

        [Header("Interaction")]
        [SerializeField] private KeyCode keyToInteract = KeyCode.E;
        [SerializeField] private Image keyInteractionImage;

        [Header("Camera Configuration")]
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float rotationSpeed = 3f;

        private bool isNearInteractuable = false;
        private IInteractuable currentInteractuable;

        private Coroutine rotationCoroutine;

        private void StartInteraction() 
        {
            var posToLook = currentInteractuable.PositionToFace();
            LookAtSmoothly(posToLook.position);

            firstPersonController.enabled = false;
            keyInteractionImage.gameObject.SetActive(false);
            currentInteractuable.OnInteraction();
            currentInteractuable.OffInteraction(EndInteraction);
        }

        private void EndInteraction() 
        {
            firstPersonController.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            currentInteractuable = other.GetComponent<IInteractuable>();
            if (currentInteractuable != null)
            {
                isNearInteractuable = true;
                keyInteractionImage.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            isNearInteractuable = false;
            currentInteractuable = null;
            keyInteractionImage.gameObject.SetActive(false);
        }

        private void Update()
        { 
            if (isNearInteractuable && Input.GetKeyDown(keyToInteract)) 
            {
                StartInteraction();
            }
        }

        private Coroutine lookCoroutine;

        /// <summary>
        /// Inicia una rotación suave hacia el punto indicado.
        /// </summary>
        private void LookAtSmoothly(Vector3 worldPoint)
        {
            if (targetCamera == null)
                targetCamera = Camera.main;

            // Si ya hay una coroutine en curso, cancelamos la anterior
            if (rotationCoroutine != null)
                StopCoroutine(rotationCoroutine);

            rotationCoroutine = StartCoroutine(RotateToTarget(worldPoint));
        }

        private IEnumerator RotateToTarget(Vector3 worldPoint)
        {
            if (targetCamera == null) yield break;

            Transform camTransform = targetCamera.transform;

            while (true)
            {
                Vector3 direction = worldPoint - camTransform.position;

                if (direction == Vector3.zero)
                    yield break;

                Quaternion targetRotation = Quaternion.LookRotation(direction);

                camTransform.rotation = Quaternion.Slerp(
                    camTransform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

                if (Quaternion.Angle(camTransform.rotation, targetRotation) < 0.5f)
                    break;

                yield return null;
            }

            // Ajuste final
            Vector3 finalDir = worldPoint - camTransform.position;
            camTransform.rotation = Quaternion.LookRotation(finalDir);

            rotationCoroutine = null;
        }

        /// <summary>
        /// Detiene la rotación actual si está en curso.
        /// </summary>
        public void StopLooking()
        {
            if (lookCoroutine != null)
            {
                StopCoroutine(lookCoroutine);
                lookCoroutine = null;
            }
        }
    }
}