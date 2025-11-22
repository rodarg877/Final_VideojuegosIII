using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("0: Humanos, 1: Elfos, 2: Orcos, 3: Enanos")]
    public int biomeID;
    
    // Opcional: Si quieres que al salir se calle todo (ir a silencio)
    public bool silenciarAlSalir = false; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Llamamos al DJ Global para que haga el crossfade a este bioma
            if (MusicManager.instance != null)
            {
                MusicManager.instance.CambiarBioma(biomeID);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (silenciarAlSalir && other.CompareTag("Player"))
        {
            // Puedes crear un método en MusicManager para ir a silencio
            // O simplemente dejar que el siguiente trigger se encargue
        }
    }
}