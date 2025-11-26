using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Regadera : ObjetoInteractuable
{
    [Header("Capacidad de agua")]
    public int aguaMaxima = 15;
    public int aguaActual = 0;
    public int aguaRecarga = 15;

    [Header("UI")]
    public Image barra;

    [Header("Recarga")]
    public float tiempoRecarga = 2f;

    // Anti-Spam
    public float intervaloRecarga = 1f;
    private float ultimaRecarga = -999f;

    public bool TieneAgua => aguaActual > 0;

    private void Start()
    {
        ActualizarBarraAgua();
    }

    // ------------ RECARGAR AGUA -----------------

    public bool RecargarAgua()
    {
        if (Time.time - ultimaRecarga < intervaloRecarga)
            return false;

        if (aguaActual >= aguaMaxima)
            return false;

        ultimaRecarga = Time.time;

        NuevoMovimiento jugador = FindObjectOfType<NuevoMovimiento>();
        if (jugador != null)
            jugador.StartCoroutine(ProcesoRecarga(jugador));

        return true;
    }

    private IEnumerator ProcesoRecarga(NuevoMovimiento jugador)
    {
        AudioManager.instance.SonidoRecargarAgua();

        yield return new WaitForSeconds(tiempoRecarga);

        aguaActual = Mathf.Min(aguaActual + aguaRecarga, aguaMaxima);
        ActualizarBarraAgua();
    }

    // ------------ USAR AGUA -----------------

    public bool UsarAgua()
    {
        if (aguaActual <= 0)
            return false;

        aguaActual--;
        ActualizarBarraAgua();

        return true;
    }

    private void ActualizarBarraAgua()
    {
        if (barra != null)
            barra.fillAmount = (float)aguaActual / aguaMaxima;
    }
}
