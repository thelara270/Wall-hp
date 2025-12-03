using UnityEngine;

public class ZonaElectrificada : MonoBehaviour
{
    public enum LadoZona { Izquierda, Derecha }
    public LadoZona ladoZona;

    [Header("Partículas")]
    public ParticleSystem particulas;

    [Header("Daño al jugador")]
    public int dañoPorSegundo = 10;

    private bool activa = false;
    bool jugadorDentro = false;

    float tiempoAcumulado = 0f;
    ControladorVida jugador;

    void Update()
    {
        if (!activa)
            return;

        if (jugadorDentro)
        {
            tiempoAcumulado += Time.deltaTime;

            if (tiempoAcumulado >= 1f)
            {
                tiempoAcumulado = 0f;
                if (jugador != null)
                    jugador.RecibirDaño(dañoPorSegundo);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var vida = other.GetComponent<ControladorVida>();
        if (vida != null)
        {
            jugador = vida;
            jugadorDentro = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var vida = other.GetComponent<ControladorVida>();
        if (vida != null)
        {
            jugadorDentro = false;
            jugador = null;
        }
    }

    public void ActivarZona()
    {
        activa = true;

        if (particulas != null)
        {
            particulas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particulas.Play(true);
        }

        Debug.Log("Zona ACTIVADA: " + name);
    }

    public void DesactivarZona()
    {
        activa = false;

        if (particulas != null)
            particulas.Stop();

        Debug.Log("Zona DESACTIVADA: " + name);
    }
}
