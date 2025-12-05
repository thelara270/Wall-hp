using System.Collections;
using UnityEngine;

public class Curacion : MonoBehaviour
{
    public GameObject panelEnfermeria;
    public float tiempoCierreAutomatico = 1f;

    public GameManager gameManager;
    public Animator animatorCuracion;

    private NuevoMovimiento movimientoJugador;
    private Animator animJugador;
    private Rigidbody rbJugador;

    private bool enUso = false;

    private void OnTriggerEnter(Collider other)
    {
        if (enUso)
            return;

        ControladorVida vida = other.GetComponent<ControladorVida>();
        if (vida == null)
            return;

        movimientoJugador = other.GetComponent<NuevoMovimiento>();
        animJugador = other.GetComponent<Animator>();
        rbJugador = other.GetComponent<Rigidbody>();

        // SOLO CURA SI FALTA VIDA
        if (vida.vidaActual >= vida.vidaMaxima)
            return;

        enUso = true;

        // BLOQUEAR PAUSA
        gameManager.bloqueoPausa = true;

        // BLOQUEAR MOVIMIENTO DEL SCRIPT
        if (movimientoJugador != null)
            movimientoJugador.enabled = false;

        // BLOQUEAR ROOT MOTION
        //if (animJugador != null)
        //{
        //    animJugador.applyRootMotion = false;
        //    animJugador.SetFloat("Velocidad", 0f);
        //}

        // FRENAR RIGIDBODY + KINEMATIC
        if (rbJugador != null)
        {
            //rbJugador.velocity = Vector3.zero;
            //rbJugador.angularVelocity = Vector3.zero;
            rbJugador.isKinematic = true;   // 🔥 IMPORTANTE: evita que siga moviéndose
        }

        // ACTIVAR ANIMACIÓN DE CURACIÓN
        if (animatorCuracion != null)
            animatorCuracion.SetBool("Curando", true);

        // CURAR
        vida.Curarse(vida.vidaMaxima);

        // MOSTRAR PANEL
        panelEnfermeria.SetActive(true);

        StartCoroutine(CerrarEnfermeria());
    }

    private IEnumerator CerrarEnfermeria()
    {
        yield return new WaitForSeconds(tiempoCierreAutomatico);

        panelEnfermeria.SetActive(false);

        if (animatorCuracion != null)
            animatorCuracion.SetBool("Curando", false);

        //// DESBLOQUEAR ROOT MOTION
        //if (animJugador != null)
        //    animJugador.applyRootMotion = true;

        // DESBLOQUEAR MOVIMIENTO
        if (movimientoJugador != null)
            movimientoJugador.enabled = true;

        // RESTAURAR RIGIDBODY
        if (rbJugador != null)
            rbJugador.isKinematic = false;

        // DESBLOQUEAR PAUSA
        gameManager.bloqueoPausa = false;

        enUso = false;
    }
}
