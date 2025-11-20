using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMisionIndividual : MonoBehaviour
{
    [Header("Referencias de UI")]
    public Button botonMision;          // Referencia al botón principal
    public Image iconoMision;           // Icono principal de la misión
    public GameObject bloqueo;          // Imagen o panel que bloquea el botón
    public TextMeshProUGUI textoNombre; // Texto del nombre de la misión

    [Header("Referencias de datos")]
    public MisionBase mision;           // Referencia a la misión correspondiente

    [Header("Iconos según estado")]
    public Sprite iconoDisponible;      // Sprite cuando la misión está activa/en progreso
    public Sprite iconoCompletado;      // Sprite cuando la misión se completa

    private void Start()
    {
        // Inicializa el texto y el estado visual según la misión asignada
        if (mision != null)
        {
            textoNombre.text = mision.nombreMision;

            // Suscribirse a los eventos de la misión
            mision.onMisionIniciada.AddListener(OnMisionIniciada);
            mision.onMisionCompletada.AddListener(OnMisionCompletada);
        }

        // Actualiza visualmente al inicio
        ActualizarVisual();
    }

    private void OnDestroy()
    {
        // Se desuscribe de los eventos al destruir el objeto
        if (mision != null)
        {
            mision.onMisionIniciada.RemoveListener(OnMisionIniciada);
            mision.onMisionCompletada.RemoveListener(OnMisionCompletada);
        }
    }

    private void OnMisionIniciada()
    {
        // Cuando la misión se inicia, cambia a En Progreso
        mision.estado = EstadoMision.EnProgreso;
        ActualizarVisual();
    }

    private void OnMisionCompletada()
    {
        // Cuando la misión se completa, actualiza su estado visual
        mision.estado = EstadoMision.Completada;
        ActualizarVisual();
    }

    // Actualiza los elementos visuales según el estado actual de la misión
    public void ActualizarVisual()
    {
        switch (mision.estado)
        {
            case EstadoMision.Inactiva:
                botonMision.interactable = false;   // No se puede presionar
                bloqueo.SetActive(true);            // Muestra la imagen de bloqueo
                iconoMision.color = Color.gray;     // Color desaturado opcional
                break;

            case EstadoMision.EnProgreso:
                botonMision.interactable = true;    // Se puede presionar
                bloqueo.SetActive(false);           // Oculta el bloqueo
                iconoMision.sprite = iconoDisponible;
                iconoMision.color = Color.white;    // Restaura el color
                break;

            case EstadoMision.Completada:
                botonMision.interactable = false;   // Ya no se puede presionar
                bloqueo.SetActive(false);           // Ya no está bloqueada
                iconoMision.sprite = iconoCompletado;
                iconoMision.color = Color.white;
                break;
        }
    }
}
