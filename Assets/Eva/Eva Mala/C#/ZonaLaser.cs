using UnityEngine;

// ZonaLaser: zona que aplica daño continuo (láser) y reproduce partículas.
public class ZonaLaser : MonoBehaviour
{
    // Partículas asociadas a la zona
    public ParticleSystem particulas;

    // Daño por segundo que inflige el láser
    public int dañoPorSegundo = 20;

    // Estado de la zona
    private bool activa = false;

    // Estado del jugador dentro de la zona
    bool jugadorDentro = false;

    // Acumulador de tiempo para daño por segundo
    float tiempoAcumulado = 0f;

    // Referencia al controlador de vida del jugador
    ControladorVida jugador;

    // Actualización por frame
    void Update()
    {
        // Si la zona no está activa no hace nada
        if (!activa) return;

        // Si el jugador está dentro, acumula tiempo y aplica daño por segundo
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

    // Detecta entrada de colisionador con el jugador
    private void OnTriggerEnter(Collider other)
    {
        var vida = other.GetComponent<ControladorVida>();
        if (vida != null)
        {
            jugador = vida;
            jugadorDentro = true;
        }
    }

    // Detecta salida de colisionador del jugador
    private void OnTriggerExit(Collider other)
    {
        var vida = other.GetComponent<ControladorVida>();
        if (vida != null)
        {
            jugadorDentro = false;
            jugador = null;
        }
    }

    // Activa la zona y reproduce partículas
    public void ActivarZona()
    {
        activa = true;
        tiempoAcumulado = 0f;
        if (particulas != null)
        {
            particulas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particulas.Play(true);
        }
        Debug.Log("ZonaLaser ACTIVADA: " + name);
    }

    // Desactiva la zona y detiene partículas
    public void DesactivarZona()
    {
        activa = false;
        if (particulas != null)
            particulas.Stop();
        Debug.Log("ZonaLaser DESACTIVADA: " + name);
    }
}
