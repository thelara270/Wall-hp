using UnityEngine;
using UnityEngine.UI;

public class ControladorVidaEnemigo : MonoBehaviour
{
    [Header("Ajustes de Vida")]
    public int vidaMaxima = 100;
    public int vidaActual = 100;

    [Header("UI - Barra de Vida (Filled)")]
    public Image barraVida; // barra opcional (encima del enemigo)

    private bool estaMuerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;

        // Si la barra no está asignada en el inspector, intenta buscarla por tag
        if (barraVida == null)
        {
            GameObject barraObj = GameObject.FindGameObjectWithTag("BarraVidaEnemigo");
            if (barraObj != null)
            {
                barraVida = barraObj.GetComponent<Image>();
            }
        }

        ActualizarBarraVida();
    }

    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Morir();
            return;
        }

        ActualizarBarraVida();
    }

    void Morir()
    {
        estaMuerto = true;
        Debug.Log("ENEMIGO MUERTO: " + gameObject.name);
        Destroy(gameObject); // o animación o desactivar AI
    }

    void ActualizarBarraVida()
    {
        if (barraVida != null)
        {
            barraVida.fillAmount = (float)vidaActual / vidaMaxima;
        }
    }
}
