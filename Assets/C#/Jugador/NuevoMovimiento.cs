using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuevoMovimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;
    public float velocidadAgachado = 3f;

    [Header("Salto")]
    public float fuerzaSalto = 10f;

    [Header("Detección de Suelo")]
    public Transform centroDeteccion;
    public Vector3 tamanoDeteccion = new Vector3(0.5f, 0.1f, 0.5f);
    public LayerMask capaSuelo;

    private Rigidbody rb;
    private Animator animator;

    // Para notificar sólo una vez por requisito (evitar repetidos)
    private bool movimientoNotificado = false;
    private bool saltoNotificado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // El mouse ya no controla nada
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        Mover();
        Saltar();
        SaltoYCaida();
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Dirección basada en la cámara fija
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Quitamos componente vertical para que no mueva hacia arriba/abajo
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Dirección final según la cámara
        Vector3 direccion = (camForward * vertical) + (camRight * horizontal);

        // Rotación hacia la dirección del movimiento
        if (direccion.magnitude > 0.1f)
        {
            Quaternion rotObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotObjetivo, 10f * Time.deltaTime);
        }

        float velocidadFinal = Input.GetKey(KeyCode.LeftShift) ? velocidadCorrer : velocidad;

        // Aplicar movimiento
        Vector3 movimiento = direccion.normalized * velocidadFinal;
        rb.velocity = new Vector3(movimiento.x, rb.velocity.y, movimiento.z);

        // Animaciones
        float velHorizontal = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        if (animator) animator.SetFloat("Velocidad", velHorizontal);
        if (animator) animator.SetBool("Corriendo", Input.GetKey(KeyCode.LeftShift));

        // Notificar movimiento (solo una vez)
        if (!movimientoNotificado && direccion.magnitude > 0.1f)
        {
            movimientoNotificado = true;
            DialogoManager.instancia?.CumplirRequisito();
        }
    }

    bool EstaEnElSuelo()
    {
        Collider[] colisiones = Physics.OverlapBox(
            centroDeteccion.position,
            tamanoDeteccion / 2f,
            Quaternion.identity,
            capaSuelo
        );

        return colisiones.Length > 0;
    }

    void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && EstaEnElSuelo())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);

            // Notificar salto (solo una vez)
            if (!saltoNotificado)
            {
                saltoNotificado = true;
                DialogoManager.instancia?.CumplirRequisito();
            }
        }
    }

    void SaltoYCaida()
    {
        if (!EstaEnElSuelo())
        {
            if (rb.velocity.y > 0.1f)
            {
                if (animator) animator.SetBool("Saltando", true);
                if (animator) animator.SetBool("Cayendo", false);
            }
            else if (rb.velocity.y < -0.1f)
            {
                if (animator) animator.SetBool("Saltando", false);
                if (animator) animator.SetBool("Cayendo", true);
            }
        }
        else
        {
            if (animator) animator.SetBool("Saltando", false);
            if (animator) animator.SetBool("Cayendo", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (centroDeteccion == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(centroDeteccion.position, tamanoDeteccion);
    }
}
