using UnityEngine;

public class SpawnerJugador : MonoBehaviour
{
    public GameObject[] personajes; // Lista de prefabs de personajes (mismos que en la selección)
    public Transform puntoInicio;   // Punto donde aparecerá el jugador al iniciar

    private void Start()
    {
        // Obtiene el índice del personaje guardado previamente
        int indice = DatosJugador.instancia.indicePersonaje;

        // Comprueba que el índice sea válido antes de instanciar
        if (indice >= 0 && indice < personajes.Length)
        {
            // Instancia el personaje en la posición indicada
            GameObject jugador = Instantiate(personajes[indice], puntoInicio.position, Quaternion.identity);
            Debug.Log("a ver");
            // Asigna el nombre guardado al GameObject del jugador
            jugador.name = DatosJugador.instancia.nombreJugador;
        }
        else
        {
            Debug.LogWarning("Índice de personaje inválido");
        }
    }
}
