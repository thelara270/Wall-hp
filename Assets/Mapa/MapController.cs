using UnityEngine;

public class MapController : MonoBehaviour
{
    private Animator animator;

    [Header("Tecla del mapa")]
    public KeyCode teclaMapa = KeyCode.M;

    [Header("Tiempo antes de desaparecer automáticamente")]
    public float tiempoAutoCerrar = 3f;

    private bool mapaVisible = false;
    private bool mapaAgrandado = false;
    private float timer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // aseguramos que inicia invisible
        animator.SetBool("visible", false);
        animator.SetBool("agrandado", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(teclaMapa))
        {
            if (!mapaVisible)
            {
                MostrarMapa();
            }
            else if (!mapaAgrandado)
            {
                AgrandarMapa();
            }
        }

        // temporizador si está visible
        if (mapaVisible)
        {
            timer += Time.deltaTime;

            if (timer >= tiempoAutoCerrar)
            {
                OcultarMapa();
            }
        }
    }

    private void MostrarMapa()
    {
        mapaVisible = true;
        mapaAgrandado = false;
        timer = 0f;

        animator.SetBool("visible", true);      // activa anim de aparecer
        animator.SetBool("agrandado", false);
    }

    private void AgrandarMapa()
    {
        mapaAgrandado = true;
        timer = 0f;

        animator.SetBool("agrandado", true);    // activa anim de agrandar
    }

    private void OcultarMapa()
    {
        mapaVisible = false;
        mapaAgrandado = false;
        timer = 0f;

        animator.SetBool("visible", false);     // activa anim de desaparecer
        animator.SetBool("agrandado", false);
        animator.SetTrigger("desaparecer");
    }
}
