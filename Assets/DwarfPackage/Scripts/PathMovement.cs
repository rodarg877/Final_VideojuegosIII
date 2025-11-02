using UnityEngine;
using System.Collections;

namespace DwarvenPackage
{
    public class PathMovement : MonoBehaviour
    {
        [Header("Ruta del NPC")]
        [SerializeField] private Transform[] waypoints;      // Puntos del recorrido
        [SerializeField] private float speed = 2f;           // Velocidad de movimiento
        [SerializeField] private float distanceForContinue = 1f;

        [Header("Visualización")]
        [SerializeField] private Color pathColor = Color.yellow;
        [SerializeField] private Color pointColor = Color.cyan;
        [SerializeField] private float pointSize = 0.2f;

        private int currentIndex = 0;
        private bool movingForward = true;

        private void Start()
        {
            if (waypoints.Length > 0)
                transform.position = waypoints[0].position;
        }

        private void Update()
        {
            if (waypoints.Length < 2) return;

            MoveAlongPath();
        }

        private void MoveAlongPath()
        {
            Transform target = waypoints[currentIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            // Rotación automática hacia el siguiente punto
            Vector3 dir = (target.position - transform.position).normalized;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);

            // Si llegó al punto destino
            if (Vector3.Distance(transform.position, target.position) <= distanceForContinue)
            {
                WaitAndContinue();
            }
        }

        private void WaitAndContinue()
        {
            if (movingForward)
            {
                if (currentIndex < waypoints.Length - 1)
                    currentIndex++;
                else
                    movingForward = false; // Cambia dirección
            }
            else
            {
                if (currentIndex > 0)
                    currentIndex--;
                else
                    movingForward = true; // Cambia dirección
            }
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Length < 2) return;

            // Dibuja líneas y puntos
            Gizmos.color = pathColor;
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                if (waypoints[i] && waypoints[i + 1])
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }

            Gizmos.color = pointColor;
            foreach (var wp in waypoints)
            {
                if (wp)
                    Gizmos.DrawSphere(wp.position, pointSize);
            }
        }
    }

}
