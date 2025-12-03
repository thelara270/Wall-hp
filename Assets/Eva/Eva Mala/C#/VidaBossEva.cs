using UnityEngine;
using UnityEngine.UI;

public class VidaBossEva : MonoBehaviour
{
    public int vidaMaxima = 300;
    public int vidaActual;
    [HideInInspector] public bool destruida = false;

    public bool puedeRecibirDaño = false;

    public Image barraVida;

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    public void ActivarDaño()
    {
        puedeRecibirDaño = true;
    }

    public void DesactivarDaño()
    {
        puedeRecibirDaño = false;
    }

    public void RecibirDaño(int cantidad)
    {
        if (!puedeRecibirDaño) return;
        if (destruida) return;

        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            destruida = true;
        }

        ActualizarBarra();
    }

    public void SetVidaMaxima(int max)
    {
        vidaMaxima = max;
        vidaActual = vidaMaxima;
        ActualizarBarra();
    }

    void ActualizarBarra()
    {
        if (barraVida != null)
            barraVida.fillAmount = (float)vidaActual / (float)vidaMaxima;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!puedeRecibirDaño) return;

        DañoEnemigo daño = other.GetComponent<DañoEnemigo>();
        if (daño != null)
            RecibirDaño(daño.daño);
    }
}
