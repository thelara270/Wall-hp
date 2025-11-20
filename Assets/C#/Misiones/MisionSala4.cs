using UnityEngine;

// Clase que representa la misión correspondiente a la Sala 4,
// donde el jugador debe completar un rompecabezas (puzzle).
public class MisionSala4 : MisionBase
{
    [Header("Referencias")]
    public PuzzleManager puzzleManager;   // Referencia al gestor del puzzle

    private bool misionActiva = false;    // Indica si la misión ya fue activada

    private void Start()
    {
        // Inicialmente la misión está inactiva
        estado = EstadoMision.Inactiva;

        // Se suscribe al evento global del GestorMisiones
        if (GestorMisiones.Instance != null)
            GestorMisiones.Instance.OnMisionCompletada += RevisarDesbloqueo;
    }

    // Se ejecuta cuando cualquier misión se completa
    private void RevisarDesbloqueo(MisionBase misionTerminada)
    {
        // Si la misión completada fue la Sala 3, se activa esta
        if (misionTerminada is MisionSala3)
        {
            // Se desuscribe para evitar múltiples activaciones
            GestorMisiones.Instance.OnMisionCompletada -= RevisarDesbloqueo;

            // Activa esta misión
            GestorMisiones.Instance.ActivarMision(this);
            misionActiva = true;

            Debug.Log("Misión 4 (Puzzle) activada: resuelve el rompecabezas.");

            // Se suscribe al evento del PuzzleManager
            if (puzzleManager != null)
                puzzleManager.OnPuzzleCompletado += CompletarMision;
        }
    }

    // En esta misión no hay verificación progresiva continua
    public override void VerificarProgreso() { }

    // Completa la misión y se desuscribe del evento del puzzle
    public override void CompletarMision()
    {
        base.CompletarMision();

        if (puzzleManager != null)
            puzzleManager.OnPuzzleCompletado -= CompletarMision;

        Debug.Log("Has completado la misión de la Sala 4 tras resolver el puzzle.");
    }
}
