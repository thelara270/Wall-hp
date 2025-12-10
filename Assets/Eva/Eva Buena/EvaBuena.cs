using UnityEngine;
using UnityEngine.AI;

public class EvaBuena : MonoBehaviour
{
    [Header("Configuración")]
    public string tagJugador = "Player";
    public float distanciaMinima = 2f;
    public float distanciaExtra = 0.5f;
    public float velocidad = 3.5f;
    public float rotacionSuave = 8f;

    [Header("Detección de proximidad (con histéresis)")]
    public float radioEntrada = 1.2f;    // entra en modo alejarse
    public float radioSalida = 1.8f;     // sale del modo solo cuando está lo bastante lejos
    public float alturaSphere = 1.0f;
    public float distanciaAlejar = 3f;

    private Transform jugador;
    private NavMeshAgent agente;
    private Animator anim;

    private bool jugadorDemasiadoCerca = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agente.speed = velocidad;
        agente.stoppingDistance = distanciaMinima + distanciaExtra;
        agente.updateRotation = false;

        BuscarJugador();
    }

    void Update()
    {
        if (jugador == null)
        {
            BuscarJugador();
            return;
        }

        DetectarProximidad();

        if (jugadorDemasiadoCerca)
            AlejarseDelJugador();
        else
            SeguirJugador();

        RotacionSuave();
    }

    void BuscarJugador()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tagJugador);
        if (obj != null)
            jugador = obj.transform;
    }

    // -----------------------------------------
    // DETECCIÓN CON 2 RADIOS (evita el ciclo)
    // -----------------------------------------
    void DetectarProximidad()
    {
        Vector3 pos = transform.position + Vector3.up * alturaSphere;
        float dist = Vector3.Distance(pos, jugador.position);

        if (!jugadorDemasiadoCerca)
        {
            // Aún no está marcado como cerca → verifica si entra
            if (dist <= radioEntrada)
                jugadorDemasiadoCerca = true;
        }
        else
        {
            // Ya está marcado como cerca → solo sale cuando se aleja más
            if (dist >= radioSalida)
                jugadorDemasiadoCerca = false;
        }
    }

    void AlejarseDelJugador()
    {
        Vector3 direccion = (transform.position - jugador.position).normalized;
        Vector3 destino = transform.position + direccion * distanciaAlejar;

        agente.SetDestination(destino);

        if (anim != null)
            anim.SetBool("Caminando", true);
    }

    void SeguirJugador()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia > distanciaMinima + distanciaExtra)
        {
            agente.SetDestination(jugador.position);
            anim?.SetBool("Caminando", true);
        }
        else
        {
            agente.ResetPath();
            anim?.SetBool("Caminando", false);
        }
    }

    void RotacionSuave()
    {
        Vector3 vel = agente.velocity;
        vel.y = 0;

        if (vel.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(vel);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotacionSuave);
        }
    }

    // Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * alturaSphere, radioEntrada);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * alturaSphere, radioSalida);
    }
}
