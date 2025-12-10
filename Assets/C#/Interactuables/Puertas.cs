using UnityEngine;

public class Puertas : MonoBehaviour
{
    private Animator animator;

    [Header("Configuración")]
    public string tagJugador = "Player";
    public float tiempoParaCerrar = 3f;
    public bool bloqueada = true; 

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
        AudioManager.instance.SonidoPuertas();

        // Se cerrará automáticamente después de X segundos
        Invoke(nameof(CerrarPuerta), tiempoParaCerrar);
    }

    private void CerrarPuerta()
    {
        Invoke(nameof(SonidoCerrarPuerta), 0.8f);

        puertaAbierta = false;
        animator.SetBool("Abrir", false);
    }

    private void SonidoCerrarPuerta()
    {
        AudioManager.instance.SonidoPuertas();
    }

    public void DesbloquearPuerta()
    {
        bloqueada = false;
    }
}
