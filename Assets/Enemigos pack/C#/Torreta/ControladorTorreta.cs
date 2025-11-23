using UnityEngine;
using System.Collections;


public class ControladorTorreta : MonoBehaviour
{
    public Transform jugador;
    public float rangoDeteccion = 10f;
    public Transform puntoDisparo;

    public float tiempoAntesDeDisparar = 2f;
    public Light luzDeteccion;

    [HideInInspector] public Animator animator;

    private EstadoTorreta estadoActual;
    private EstadoTorretaIdle estadoIdle;
    private EstadoTorretaDetectar estadoDetectar;
    private EstadoTorretaDisparar estadoDisparar;

    void Start()
    {
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                jugador = playerObj.transform;
            else
                StartCoroutine(EsperarJugador());
        }

        animator = GetComponent<Animator>();

        estadoIdle = new EstadoTorretaIdle(this);
        estadoDetectar = new EstadoTorretaDetectar(this);
        estadoDisparar = new EstadoTorretaDisparar(this);

        ChangeState(estadoIdle);
    }
    IEnumerator EsperarJugador()
    {
        while (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                jugador = playerObj.transform;
                Debug.Log("🔹 Jugador asignado al enemigo patrulla.");
            }
            yield return null; // Espera un frame
        }
    }

    void Update()
    {
        estadoActual?.Update();
    }

    public void ChangeState(EstadoTorreta nuevoEstado)
    {
        estadoActual?.Exit();
        estadoActual = nuevoEstado;
        nuevoEstado.Enter();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }

    public EstadoTorretaIdle GetIdleState() => estadoIdle;
    public EstadoTorretaDetectar GetDetectState() => estadoDetectar;
    public EstadoTorretaDisparar GetShootState() => estadoDisparar;
}
