using UnityEngine;

public class ControladorExplosivo : MonoBehaviour
{
    private EstadoExplosivo estadoActual;

    [HideInInspector] public Transform jugador;
    public Animator animator;

    [Header("Parametros")]
    public float radioDeteccion = 5f;
    public float tiempoCarga = 1.5f;
    public float velocidadAtaque = 8f;
    public float distanciaExplosion = 1.2f;
    public int daño = 5;

    [Header("Referencias")]
    public GameObject particulasExplosion;

    [Header("Tiempo de vida")]
    public float tiempoMaximoVivo = 6f;
    private float vidaActual = 0f;

    [Header("Detección de obstáculos")]
    public float radioDeteccionPared = 1f;
    public string tagParedes = "Pared";

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = false;

        estadoActual = new EstadoExplosivoIdle(this);
        estadoActual.Enter();
    }

    void Update()
    {
        vidaActual += Time.deltaTime;
        if (vidaActual >= tiempoMaximoVivo)
        {
            Debug.Log($"Explosivo {name}: tiempo agotado -> explota sin daño");
            Explode(false);
            return;
        }

        estadoActual.Update();
    }

    public void ChangeState(EstadoExplosivo nuevoEstado)
    {
        estadoActual?.Exit();
        estadoActual = nuevoEstado;
        nuevoEstado.Enter();
    }

    public void Explode(bool dañarJugador = false)
    {
        Instantiate(particulasExplosion, transform.position, Quaternion.identity);

        if (dañarJugador && jugador != null)
        {
            ControladorVida vidaJugador = jugador.GetComponent<ControladorVida>();
            if (vidaJugador != null)
            {
                vidaJugador.RecibirDaño(daño); // ← Ajusta daño aquí
                Debug.Log($"💥 Explosivo {name} hizo daño al jugador.");
            }
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log($"💥 Explosivo {name} impactó al jugador -> explota con daño");
            Explode(true);
        }
        else if (collision.collider.CompareTag(tagParedes))
        {
            Debug.Log($"🧱 Explosivo {name} chocó con pared -> explota sin daño");
            Explode(false);
        }
        else
        {
            Debug.Log($"🧨 Explosivo {name} chocó con {collision.collider.name} -> explota sin daño");
            Explode(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaExplosion);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radioDeteccionPared);
    }
}
