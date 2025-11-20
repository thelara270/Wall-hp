using UnityEngine;

public class PanelElectrico : MonoBehaviour
{
    public MinijuegoCables minijuego; //Referencia al minijuego

    //Detecta si el jugador esta en rango
    private bool enRango;

    private void Update()
    {
        //Si el jugador esta en rango y presiona la tecla E 
        if (enRango && Input.GetKeyDown(KeyCode.E))
        {
            //inicia el minijuego
            minijuego.IniciarMinijuego();
        }
    }

    public void Reparar()
    {
        Debug.Log("El panel electrico ha sido reparado");
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
