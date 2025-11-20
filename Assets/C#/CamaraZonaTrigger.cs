using UnityEngine;
using Cinemachine;

public class CamaraZonaTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera camaraDeZona;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camaraDeZona.Priority = 20; // Activar esta cámara
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camaraDeZona.Priority = 0; // Volver a la cámara anterior
        }
    }
}