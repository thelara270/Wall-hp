using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Regadera : ObjetoInteractuable
{
    [Header("Capacidad de agua")]
    public int aguaMaxima = 15;    // Máxima cantidad de agua que puede almacenar
    public int aguaActual = 0;    // Cantidad actual de agua
    public int aguaRecarga = 15;  // Cantidad de agua recargada al llenar

    [Header("UI")]
    public Image barra;           // Barra visual de nivel de agua

    [Header("Recarga")]
    public float tiempoRecarga = 2f; // Tiempo que tarda en recargar

    public bool TieneAgua => aguaActual > 0; // Propiedad para verificar si tiene agua

    private void Start()
    {
        ActualizarBarraAgua();
    }

    // Intenta recargar la regadera si no está llena
    public bool RecargarAgua()
    {
        if (aguaActual >= aguaMaxima)
        {
            //UIInventario.Instance.MostrarMensaje("La regadera ya está llena");
            return false;
        }

        NuevoMovimiento jugador = FindObjectOfType<NuevoMovimiento>();
        if (jugador != null)
        {
            jugador.StartCoroutine(ProcesoRecarga(jugador));
        }

        return true;
    }

    // Corrutina que simula el proceso de recarga con retardo
    private IEnumerator ProcesoRecarga(NuevoMovimiento jugador)
    {
        //UIInventario.Instance.MostrarMensaje("Recargando la regadera...");

        yield return new WaitForSeconds(tiempoRecarga);

        // Suma la recarga sin exceder el máximo
        aguaActual = Mathf.Min(aguaActual + aguaRecarga, aguaMaxima);
        ActualizarBarraAgua();

        //UIInventario.Instance.MostrarMensaje("");
    }

    // Reduce la cantidad de agua al usarla
    public bool UsarAgua()
    {
        if (aguaActual > 0)
        {
            aguaActual--;
            ActualizarBarraAgua();
            return true;
        }
        else
        {
            //UIInventario.Instance.MostrarMensaje("La regadera está vacía");
            return false;
        }
    }

    // Actualiza visualmente la barra de agua
    private void ActualizarBarraAgua()
    {
        if (barra != null)
        {
            barra.fillAmount = (float)aguaActual / aguaMaxima;
        }
    }
}
