<<<<<<< HEAD
<<<<<<< HEAD
using UnityEngine;
using System.Collections;

namespace SkyrimProject.DwarvenPackage
{
    public class DwarfEntranceController : MonoBehaviour
    {
        [Header("Configuración de puertas")]
        public DwarfDoorData[] doors;

        private void Start()
        {
            // Guardamos las rotaciones iniciales
            foreach (var d in doors)
            {
                if (d.transform == null || d.hingePoint == null)
                {
                    Debug.LogWarning($"Puerta sin asignar correctamente en {gameObject.name}");
                    continue;
                }

                // Guardar rotación inicial
                d.closedRotation = d.transform.rotation;

                // Calcular la rotación abierta
                float dir = d.invertDirection ? -1f : 1f;
                d.openRotation = d.closedRotation * Quaternion.Euler(0f, dir * d.openAngle, 0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                ToggleDoors(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                ToggleDoors(false);
        }

        private void ToggleDoors(bool open)
        {
            foreach (var d in doors)
            {
                if (d.transform == null || d.hingePoint == null)
                    continue;

                if (d.coroutine != null)
                    StopCoroutine(d.coroutine);

                d.coroutine = StartCoroutine(RotateDoor(d, open));
            }
        }

        private IEnumerator RotateDoor(DwarfDoorData d, bool open)
        {
            Quaternion startRot = d.transform.rotation;
            Quaternion targetRot = open ? d.openRotation : d.closedRotation;

            while (Quaternion.Angle(d.transform.rotation, targetRot) > 0.1f)
            {
                // Rotar alrededor del punto de bisagra
                d.transform.RotateAround(
                    d.hingePoint.position,
                    Vector3.up,
                    (open ? 1f : -1f) * d.speed * Time.deltaTime * (d.invertDirection ? -1f : 1f)
                );

                yield return null;
            }

            // Asegura la posición final exacta
            d.transform.rotation = targetRot;
            d.isOpen = open;
            d.coroutine = null;
        }
    }
}
=======
using UnityEngine;
using System.Collections;

namespace SkyrimProject.DwarvenPackage
{
    public class DwarfEntranceController : MonoBehaviour
    {
        [Header("Configuración de puertas")]
        public DwarfDoorData[] doors;

        private void Start()
        {
            // Guardamos las rotaciones iniciales
            foreach (var d in doors)
            {
                if (d.transform == null || d.hingePoint == null)
                {
                    Debug.LogWarning($"Puerta sin asignar correctamente en {gameObject.name}");
                    continue;
                }

                // Guardar rotación inicial
                d.closedRotation = d.transform.rotation;

                // Calcular la rotación abierta
                float dir = d.invertDirection ? -1f : 1f;
                d.openRotation = d.closedRotation * Quaternion.Euler(0f, dir * d.openAngle, 0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                ToggleDoors(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                ToggleDoors(false);
        }

        private void ToggleDoors(bool open)
        {
            foreach (var d in doors)
            {
                if (d.transform == null || d.hingePoint == null)
                    continue;

                if (d.coroutine != null)
                    StopCoroutine(d.coroutine);

                d.coroutine = StartCoroutine(RotateDoor(d, open));
            }
        }

        private IEnumerator RotateDoor(DwarfDoorData d, bool open)
        {
            Quaternion startRot = d.transform.rotation;
            Quaternion targetRot = open ? d.openRotation : d.closedRotation;

            while (Quaternion.Angle(d.transform.rotation, targetRot) > 0.1f)
            {
                // Rotar alrededor del punto de bisagra
                d.transform.RotateAround(
                    d.hingePoint.position,
                    Vector3.up,
                    (open ? 1f : -1f) * d.speed * Time.deltaTime * (d.invertDirection ? -1f : 1f)
                );

                yield return null;
            }

            // Asegura la posición final exacta
            d.transform.rotation = targetRot;
            d.isOpen = open;
            d.coroutine = null;
        }
    }
}
>>>>>>> 83b979d6c08304e2436793e39b939edeec89c92c
=======
using UnityEngine;
using System.Collections;

namespace SkyrimProject.DwarvenPackage
{
    public class DwarfEntranceController : MonoBehaviour
    {
        [Header("Configuración de puertas")]
        public DwarfDoorData[] doors;

        private void Start()
        {
            // Guardamos las rotaciones iniciales
            foreach (var d in doors)
            {
                if (d.transform == null || d.hingePoint == null)
                {
                    Debug.LogWarning($"Puerta sin asignar correctamente en {gameObject.name}");
                    continue;
                }

                // Guardar rotación inicial
                d.closedRotation = d.transform.rotation;

                // Calcular la rotación abierta
                float dir = d.invertDirection ? -1f : 1f;
                d.openRotation = d.closedRotation * Quaternion.Euler(0f, dir * d.openAngle, 0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                ToggleDoors(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                ToggleDoors(false);
        }

        private void ToggleDoors(bool open)
        {
            foreach (var d in doors)
            {
                if (d.transform == null || d.hingePoint == null)
                    continue;

                if (d.coroutine != null)
                    StopCoroutine(d.coroutine);

                d.coroutine = StartCoroutine(RotateDoor(d, open));
            }
        }

        private IEnumerator RotateDoor(DwarfDoorData d, bool open)
        {
            Quaternion startRot = d.transform.rotation;
            Quaternion targetRot = open ? d.openRotation : d.closedRotation;

            while (Quaternion.Angle(d.transform.rotation, targetRot) > 0.1f)
            {
                // Rotar alrededor del punto de bisagra
                d.transform.RotateAround(
                    d.hingePoint.position,
                    Vector3.up,
                    (open ? 1f : -1f) * d.speed * Time.deltaTime * (d.invertDirection ? -1f : 1f)
                );

                yield return null;
            }

            // Asegura la posición final exacta
            d.transform.rotation = targetRot;
            d.isOpen = open;
            d.coroutine = null;
        }
    }
}
>>>>>>> 83b979d6c08304e2436793e39b939edeec89c92c
