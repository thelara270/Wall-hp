using UnityEngine;

public class BrazoConVida : MonoBehaviour
{
    public int vidaMaxima = 100;
    public int vidaActual = 100;

    public bool destruido = false;
    public bool puedeRecibirDaño = false; // NUEVO

    public Animator animadorBrazo;
    public string animTriggerCaer = "BrazoCaer";

    Collider col;

    void Start()
    {
        vidaActual = vidaMaxima;
        col = GetComponent<Collider>();
        puedeRecibirDaño = false;     // Desactivado al inicio
    }

    public void ActivarDaño()
    {
        puedeRecibirDaño = true;
        if (col != null) col.enabled = true;
    }

    public void DesactivarDaño()
    {
        puedeRecibirDaño = false;
        if (col != null) col.enabled = false;
    }

    public void RecibirDaño(int cantidad)
    {
        if (!puedeRecibirDaño) return;  // 🚨 NUEVO
        if (destruido) return;

        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            DestruirBrazo();
        }
    }

    void DestruirBrazo()
    {
        destruido = true;
        puedeRecibirDaño = false;

        if (col != null) col.enabled = false;

        if (animadorBrazo != null)
            animadorBrazo.SetTrigger(animTriggerCaer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!puedeRecibirDaño) return;

        DañoEnemigo daño = other.GetComponent<DañoEnemigo>();
        if (daño != null)
            RecibirDaño(daño.daño);
    }
}
