using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // IMPORTANTE

public class ControladorVida : MonoBehaviour
{
    [Header("Ajustes de Vida")]
    public int vidaMaxima = 100;
    public int vidaActual = 100;

    [Header("Curación Automática")]
    public bool curacionAutomaticaActiva = false;
    public float tiempoParaComenzarCurar = 5f;
    public int velocidadCuracion = 5;

    [Header("UI - Barra de Vida (Filled)")]
    public Image barraVida;  // **Aquí en vez de Slider**

    private float tiempoSinDaño = 0f;
    private bool recibioDañoRecientemente = false;
    private bool estaMuerto = false;

    private float acumuladorCuracion = 0f;

    void Start()
    {
        vidaActual = vidaMaxima;

        if (barraVida == null)
        {
            GameObject barraObj = GameObject.FindGameObjectWithTag("BarraVida");
            if (barraObj != null)
            {
                barraVida = barraObj.GetComponent<Image>();
            }
            else
            {
                Debug.LogWarning("⚠ No se encontró ningún objeto con el tag 'BarraVida'");
            }
        }

        ActualizarBarraVida();
    }

    void Update()
    {
        ActualizarBarraVida();

        // Control del tiempo sin recibir daño
        if (recibioDañoRecientemente)
        {
            tiempoSinDaño += Time.deltaTime;

            if (tiempoSinDaño >= tiempoParaComenzarCurar)
            {
                recibioDañoRecientemente = false;
                tiempoSinDaño = 0;
            }
        }

        // Curación automática
        if (!recibioDañoRecientemente && curacionAutomaticaActiva)
        {
            if (vidaActual < vidaMaxima)
            {
                acumuladorCuracion += velocidadCuracion * Time.deltaTime;

                if (acumuladorCuracion >= 1f)
                {
                    int puntosACurar = (int)acumuladorCuracion;
                    acumuladorCuracion -= puntosACurar;

                    vidaActual += puntosACurar;

                    if (vidaActual > vidaMaxima)
                        vidaActual = vidaMaxima;
                }
            }
        }
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

        recibioDañoRecientemente = true;
        tiempoSinDaño = 0;
    }

    public void Curarse(int cantidad)
    {
        vidaActual += cantidad;

        if (vidaActual > vidaMaxima)
            vidaActual = vidaMaxima;
    }

    public void ActivarCuracionAutomatica()
    {
        curacionAutomaticaActiva = true;
    }

    void Morir()
    {
        estaMuerto = true;
        Debug.Log("EL JUGADOR HA MUERTO");
        gameObject.SetActive(false);
    }

    void ActualizarBarraVida()
    {
        if (barraVida != null)
        {
            float fillValue = (float)vidaActual / vidaMaxima;
            barraVida.fillAmount = fillValue;
        }
    }
}
