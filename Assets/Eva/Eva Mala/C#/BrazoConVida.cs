using UnityEngine;

public class BrazoConVida : MonoBehaviour
{
    public int vidaMaxima = 100;
    public int vidaActual = 100;

    public bool destruido = false;
    public bool puedeRecibirDaño = false;

    Collider col;

    void Start()
    {
        vidaActual = vidaMaxima;
        col = GetComponent<Collider>();
        puedeRecibirDaño = false;
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
        if (!puedeRecibirDaño) return;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!puedeRecibirDaño) return;

        DañoEnemigo daño = other.GetComponent<DañoEnemigo>();
        if (daño != null)
            RecibirDaño(daño.daño);
    }
}
