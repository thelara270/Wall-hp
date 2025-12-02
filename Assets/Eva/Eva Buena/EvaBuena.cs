using UnityEngine;
using UnityEngine.AI;

public class EvaBuena : MonoBehaviour
{
    [Header("Configuración")]
    public string tagJugador = "Player";
    public float distanciaMinima = 2f;
    public float distanciaExtra = 0.5f;  // evita que quede pegada
    public float velocidad = 3.5f;
    public float rotacionSuave = 8f;

    private Transform jugador;
    private NavMeshAgent agente;
    private Animator anim;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agente.speed = velocidad;
        agente.stoppingDistance = distanciaMinima + distanciaExtra;
        agente.autoBraking = true;        // evita deslizamiento
        agente.updateRotation = false;    // rotación manual
        agente.acceleration = 50f;        // frena más rápido

        BuscarJugador();
    }

    void Update()
    {
        if (jugador == null)
        {
            BuscarJugador();
            return;
        }

        SeguirJugador();
        RotacionSuave();
    }

    void BuscarJugador()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tagJugador);
        if (obj != null)
            jugador = obj.transform;
    }

    void SeguirJugador()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia > distanciaMinima + distanciaExtra)
        {
            agente.SetDestination(jugador.position);

            if (anim != null)
                anim.SetBool("Caminando", true);
        }
        else
        {
            agente.ResetPath();

            if (anim != null)
                anim.SetBool("Caminando", false);
        }
    }

    void RotacionSuave()
    {
        Vector3 vel = agente.velocity;
        vel.y = 0;

        if (vel.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(vel);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rot,
                Time.deltaTime * rotacionSuave
            );
        }
    }
}
