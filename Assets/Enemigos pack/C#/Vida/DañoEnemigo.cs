using UnityEngine;

public class DañoEnemigo : MonoBehaviour
{
    public int daño = 20;

    private void OnCollisionEnter(Collision collision)
    {
        AplicarDaño(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        AplicarDaño(other.gameObject);
    }

    private void AplicarDaño(GameObject obj)
    {
        // 1. Si es enemigo normal
        ControladorVidaEnemigo vidaEnemigo = obj.GetComponent<ControladorVidaEnemigo>();
        if (vidaEnemigo != null)
        {
            vidaEnemigo.RecibirDaño(daño);
            return;
        }

        // 2. Si es EVA
        VidaBossEva vidaEva = obj.GetComponent<VidaBossEva>();
        if (vidaEva != null)
        {
            vidaEva.RecibirDaño(daño);
            return;
        }
    }
}
