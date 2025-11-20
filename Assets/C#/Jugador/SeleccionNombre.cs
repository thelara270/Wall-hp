using UnityEngine;
using TMPro;

public class SeleccionNombre : MonoBehaviour
{
    public TMP_InputField campoNombre; // Campo de texto donde el jugador escribe su nombre

    // Guarda el nombre ingresado en los datos globales del jugador
    public void GuardarNombre()
    {
        if (!string.IsNullOrEmpty(campoNombre.text)) // Si el campo no está vacío
        {
            DatosJugador.instancia.nombreJugador = campoNombre.text; // Guarda el nombre
            Debug.Log("Nombre guardado: " + DatosJugador.instancia.nombreJugador);
        }
    }
}
