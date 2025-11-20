using UnityEngine;
using UnityEngine.Events;

// Enumeración con los posibles estados de una misión
public enum EstadoMision
{
    Inactiva,     // La misión no ha comenzado
    EnProgreso,   // La misión está en curso
    Completada    // La misión fue terminada
}

// Clase base abstracta de la cual heredan todas las misiones
public abstract class MisionBase : MonoBehaviour
{
    [Header("Datos de la misión")]
    public string nombreMision;               // Nombre identificador de la misión
    [TextArea]
    public string descripcion;                // Descripción de la misión
    public EstadoMision estado = EstadoMision.Inactiva; // Estado actual de la misión

    [Header("Eventos de la misión")]
    public UnityEvent onMisionIniciada;       // Se ejecuta al iniciar la misión
    public UnityEvent onMisionCompletada;     // Se ejecuta al completar la misión

    // Método virtual para iniciar la misión
    public virtual void IniciarMision()
    {
        // Cambia el estado a "En Progreso"
        estado = EstadoMision.EnProgreso;

        // Muestra un mensaje en pantalla (opcional)
        UIJugadorMensajes.Instance.MostrarMensaje("Misión iniciada: " + nombreMision);

        // Dispara el evento configurable en el inspector
        onMisionIniciada?.Invoke();
    }

    // Método virtual para completar la misión
    public virtual void CompletarMision()
    {
        // Cambia el estado a "Completada"
        estado = EstadoMision.Completada;

        // Muestra un mensaje en pantalla
        UIJugadorMensajes.Instance.MostrarMensaje("Misión completada: " + nombreMision);

        // Llama al evento de Unity para permitir acciones personalizadas
        onMisionCompletada?.Invoke();

        // Notifica al gestor que la misión fue completada
        GestorMisiones.Instance.NotificarMisionCompletada(this);
    }

    // Método abstracto que las misiones hijas deben implementar para verificar su progreso
    public abstract void VerificarProgreso();
}
