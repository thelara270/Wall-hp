using System.Collections;
using UnityEngine;

public class Curacion : MonoBehaviour
{
    public int cantidadCuracion;
    public GameObject panelEnfermeria;
    public float tiempoCierreAutomatico = 2f;

    public GameManager gameManager;

    private NuevoMovimiento movimientoJugador;
    private bool enUso = false;

    private void OnTriggerEnter(Collider other)
    {
        if (enUso)
            return;

        ControladorVida vida = other.GetComponent<ControladorVida>();

        if (vida == null)
            return;

        // OBTENER EL MOVIMIENTO DEL JUGADOR DESDE EL TRIGGER
        movimientoJugador = other.GetComponent<NuevoMovimiento>();

        // SOLO CURA SI LA VIDA ES MENOR AL MÁXIMO
        if (vida.vidaActual >= vida.vidaMaxima)
            return;

        enUso = true;

        // BLOQUEAR PAUSA
        gameManager.bloqueoPausa = true;

        // BLOQUEAR MOVIMIENTO DEL JUGADOR
        if (movimientoJugador != null)
            movimientoJugador.enabled = false;

        // CURAR
        vida.Curarse(cantidadCuracion);

        // MOSTRAR PANEL
        panelEnfermeria.SetActive(true);

        // EJECUTAR CIERRE AUTOMÁTICO
        StartCoroutine(CerrarEnfermeria());
    }

    private IEnumerator CerrarEnfermeria()
    {
        yield return new WaitForSeconds(tiempoCierreAutomatico);

        // OCULTAR PANEL
        panelEnfermeria.SetActive(false);

        // DESBLOQUEAR MOVIMIENTO
        if (movimientoJugador != null)
            movimientoJugador.enabled = true;

        // DESBLOQUEAR PAUSA
        gameManager.bloqueoPausa = false;

        // Destruir objeto de curación
        Destroy(gameObject);
    }
}
