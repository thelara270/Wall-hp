using UnityEngine;

public class DatosJugador : MonoBehaviour
{
    public static DatosJugador instancia; // Instancia única para guardar los datos del jugador

    public string nombreJugador = "Jugador"; // Nombre del jugador
    public int indicePersonaje = 0;          // Índice del personaje seleccionado

    private void Awake()
    {
        // Patrón Singleton: solo puede existir una instancia
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, elimina esta duplicada
        }
    }
}
