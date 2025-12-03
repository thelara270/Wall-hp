using UnityEngine;

public class Puertas : MonoBehaviour
{
    private Animator animator;

    [Header("Configuración")]
    public string tagJugador = "Player";
    public float tiempoParaCerrar = 3f;
    public bool bloqueada = true;   // <<< NUEVO — si está true NO se abre

    private bool jugadorDentro = false;
    private bool puertaAbierta = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Solo intenta abrir si el jugador está en trigger y la puerta NO está bloqueada
        if (jugadorDentro && !puertaAbierta && !bloqueada && Input.GetKeyDown(KeyCode.C))
        {
            AbrirPuerta();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorDentro = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            jugadorDentro = false;
        }
    }

    public void AbrirPuerta()
    {
        puertaAbierta = true;
        animator.SetBool("Abrir", true);

        // Se cerrará automáticamente después de X segundos
        Invoke(nameof(CerrarPuerta), tiempoParaCerrar);
    }

    private void CerrarPuerta()
    {
        puertaAbierta = false;
        animator.SetBool("Abrir", false);
    }

    public void DesbloquearPuerta()
    {
        bloqueada = false;
    }
}
