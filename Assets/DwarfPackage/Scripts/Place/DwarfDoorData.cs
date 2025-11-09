using UnityEngine;

namespace SkyrimProject.DwarvenPackage
{
    public class DwarfDoorData : MonoBehaviour
    {
        public Transform hingePoint;      // Punto de bisagra (Empty a un costado)
        public float openAngle = 90f;     // Ángulo de apertura
        public float speed = 2f;          // Velocidad de rotación
        public bool invertDirection = false; // Para abrir hacia el otro lado

        [HideInInspector] public bool isOpen = false;
        [HideInInspector] public Coroutine coroutine;
        [HideInInspector] public Quaternion closedRotation;
        [HideInInspector] public Quaternion openRotation;
    }
}