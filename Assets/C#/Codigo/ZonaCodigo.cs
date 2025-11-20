using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaCodigo : MonoBehaviour
{
    public GameObject panel; //Referencia al minijuego
    public GameObject uiJugador;

    //Detecta si el jugador esta en rango
    private bool enRango;

    private void Update()
    {
        ActivarPanel();
        DesactivarPanel();
    }

    public void ActivarPanel()
    {
        //Si el jugador esta en rango y presiona la tecla E 
        if (enRango && Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Oculta la UI del jugador y activa la del minijuego
            uiJugador.SetActive(false);
            panel.SetActive(true);
        }
    }

    public void DesactivarPanel()
    {
        if (panel != null && Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Oculta la UI del jugador y activa la del minijuego
            uiJugador.SetActive(true);
            panel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enRango = true;
            Debug.Log("Presiona E para reparar el panel electrico");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enRango = false;
        }
    }
}
