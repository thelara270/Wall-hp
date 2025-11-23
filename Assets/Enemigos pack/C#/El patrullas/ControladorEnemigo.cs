using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ControladorEnemigo : MonoBehaviour
{
    [Header("===== CONFIGURACIÓN GENERAL =====")]
    public GameObject prefabExplosivo;
    public float tiempoParaInstanciar = 3f;
    public float rangoVision = 5f;
    public float tiempoBusqueda = 3f;
    public bool buscarAlLlegar = false;

    [Header("===== POSICIONES =====")]
    public Transform jugador;
    public Transform[] puntosPatrulla;
    public Transform spawnExplosivo1;
    public Transform spawnExplosivo2;

    [Header("===== MOVIMIENTO =====")]
    public float velocidad = 2f;
    public float velocidadAngular = 120f;
    public float distanciaMinima = 2f;

    [HideInInspector] public bool enCooldown = false;
    [HideInInspector] public float tiempoPersiguiendo = 0;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Vector3 posicionInicio;
    [HideInInspector] public int indicePatrulla = 0;

    private EstadoEnemigo estadoActual;
    private EstadoEnemigoPatrullaje estadoPatrullaje;
    private EstadoEnemigoPerseguir estadoPerseguir;
    private EstadoEnemigoBuscar estadoBuscar;
    private EstadoEnemigoVolver estadoVolver;

    void Start()
    {
        // 🔹 Intentamos asignarlo si ya está en escena
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                jugador = playerObj.transform;
            else
                StartCoroutine(EsperarJugador());
        }

        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidad;
        agent.angularSpeed = velocidadAngular;

        posicionInicio = transform.position;

        estadoPatrullaje = new EstadoEnemigoPatrullaje(this);
        estadoPerseguir = new EstadoEnemigoPerseguir(this);
        estadoBuscar = new EstadoEnemigoBuscar(this);
        estadoVolver = new EstadoEnemigoVolver(this);

        ChangeState(estadoPatrullaje);
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

    void Update() => estadoActual?.Update();

    public void ChangeState(EstadoEnemigo nuevoEstado)
    {
        estadoActual?.Exit();
        estadoActual = nuevoEstado;
        estadoActual.Enter();
    }

    public EstadoEnemigoPatrullaje GetPatrolState() => estadoPatrullaje;
    public EstadoEnemigoPerseguir GetChaseState() => estadoPerseguir;
    public EstadoEnemigoBuscar GetSearchState() => estadoBuscar;
    public EstadoEnemigoVolver GetReturnState() => estadoVolver;

    public void InstanciarExplosivos()
    {
        if (prefabExplosivo == null) return;

        Instantiate(prefabExplosivo, spawnExplosivo1.position, spawnExplosivo1.rotation);
        Instantiate(prefabExplosivo, spawnExplosivo2.position, spawnExplosivo2.rotation);

        enCooldown = true;
        Invoke(nameof(ResetCooldown), 3f);
    }

    void ResetCooldown() => enCooldown = false;

    public bool DetectarJugador()
    {
        if (jugador == null) return false; // 🔹 Seguridad

        Vector3 dir = (jugador.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up, dir, out RaycastHit hit, rangoVision))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoVision);
    }
}
