using UnityEngine;

// Clase que representa la misión correspondiente a la Sala 3,
// donde el jugador debe ingresar un código correcto.
public class MisionSala3 : MisionBase
{
    [Header("Referencias")]
    public GeneradorCodigos generadorCodigos;   // Referencia al generador de códigos.

    private bool misionActiva = false;          // Indica si la misión ya está activa.

    private void Start()
    {
        // Inicializa el estado como inactivo al inicio.
        estado = EstadoMision.Inactiva;

        // Se suscribe al evento global de misión completada del Gestor de Misiones.
        if (GestorMisiones.Instance != null)
            GestorMisiones.Instance.OnMisionCompletada += RevisarDesbloqueo;
    }

    // Este método se ejecuta cuando una misión se completa en el GestorMisiones.
    private void RevisarDesbloqueo(MisionBase misionTerminada)
    {
        // Si la misión completada fue la Sala 2, activa la misión actual.
        if (misionTerminada is MisionSala2)
        {
            // Se desuscribe del evento para no repetir llamadas.
            GestorMisiones.Instance.OnMisionCompletada -= RevisarDesbloqueo;

            // Activa la misión de esta sala.
            GestorMisiones.Instance.ActivarMision(this);
            misionActiva = true;

            // Se suscribe al evento del GeneradorCodigos para detectar cuando se acierta el código.
            if (generadorCodigos != null)
                generadorCodigos.OnCodigoCorrecto += CompletarMision;
        }
    }

    // En esta misión no hay verificación progresiva, así que se deja vacío.
    public override void VerificarProgreso() { }

    // Completa la misión y se desuscribe de eventos.
    public override void CompletarMision()
    {
        // Llama a la implementación base que marca la misión como completada.
        base.CompletarMision();

        // Se desuscribe del evento del generador para evitar duplicaciones.
        if (generadorCodigos != null)
            generadorCodigos.OnCodigoCorrecto -= CompletarMision;

        // Muestra un mensaje en la consola confirmando la finalización.
        Debug.Log("Has completado la misión de la Sala 3 tras ingresar el código correcto.");
    }
}
