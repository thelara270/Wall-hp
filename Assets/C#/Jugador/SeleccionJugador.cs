using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeleccionPersonaje : MonoBehaviour
{
    [Header("Personajes disponibles")]
    public GameObject[] personajes; // Prefabs de los personajes disponibles
    private int indiceActual = 0;   // Índice del personaje actualmente mostrado

    [Header("UI")]
    public Image imagenPersonaje;   // Imagen de vista previa del personaje
    public Sprite[] spritesPreview; // Sprites que representan visualmente cada personaje
    public TMP_InputField campoNombre; // Campo para ingresar el nombre del jugador

    private void Start()
    {
        MostrarPersonaje(); // Muestra el primer personaje al iniciar
    }

    // Cambia el personaje mostrado en la vista previa (con botones de siguiente/anterior)
    public void CambiarPersonaje(int direccion)
    {
        indiceActual += direccion; // Avanza o retrocede según la dirección (+1 o -1)

        // Control de límites: si pasa del último, vuelve al primero
        if (indiceActual < 0)
            indiceActual = personajes.Length - 1;
        else if (indiceActual >= personajes.Length)
            indiceActual = 0;

        MostrarPersonaje(); // Actualiza la imagen mostrada
    }

    // Muestra el sprite del personaje actual en la UI
    void MostrarPersonaje()
    {
        if (spritesPreview.Length > 0 && imagenPersonaje != null)
            imagenPersonaje.sprite = spritesPreview[indiceActual];
    }

    // Confirma la selección del personaje y guarda los datos
    public void ConfirmarSeleccion()
    {
        // Guarda el índice del personaje en el script global
        DatosJugador.instancia.indicePersonaje = indiceActual;

        // Si el jugador escribió un nombre, también se guarda
        if (!string.IsNullOrEmpty(campoNombre.text))
            DatosJugador.instancia.nombreJugador = campoNombre.text;

        Debug.Log("Personaje seleccionado: " + indiceActual + " | Nombre: " + DatosJugador.instancia.nombreJugador);
    }
}
