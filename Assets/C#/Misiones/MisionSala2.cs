using UnityEngine;

public class MisionSala2 : MisionBase
{
    private void Start()
    {
        // Inicialmente está inactiva
        estado = EstadoMision.Inactiva;

        // Se suscribe al evento de misión completada del gestor
        if (GestorMisiones.Instance != null)
            GestorMisiones.Instance.OnMisionCompletada += RevisarDesbloqueo;
    }

    // Revisa si debe activarse al completarse otra misión
    private void RevisarDesbloqueo(MisionBase misionTerminada)
    {
        // Si la misión completada fue la Sala 1
        if (misionTerminada is MisionSala1)
        {
            // Se desuscribe del evento
            GestorMisiones.Instance.OnMisionCompletada -= RevisarDesbloqueo;

            // Activa esta misión
            GestorMisiones.Instance.ActivarMision(this);
        }
    }

    public override void VerificarProgreso()
    {
        // El progreso se controla desde GestorArboles
    }

    public override void CompletarMision()
    {
        // Llama al método base para marcar como completada
        base.CompletarMision();
        Debug.Log("Misión de la Sala 2 completada: todos los árboles crecieron");
    }
}
