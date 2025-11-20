using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;           //Velocidad normal de caminar
    public float velocidadCorrer = 8f;     //Velocidad al correr
    public float velocidadAgachado = 3f;   //velocidad reducida al agacharses

    [Header("Mouse")]
    public float sensibilidadMouse = 2f;   //Sensibilidad para rotar con el mouse

    [Header("Salto")]
    public float fuerzaSalto = 10f;        //Fuerza del salto

    [Header("Detección de Suelo")]
    public Transform centroDeteccion;                                //Punto desde el que se detecta el suelo
    public Vector3 tamanoDeteccion = new Vector3(0.5f, 0.1f, 0.5f);  //Tamaño del area de deteccion
    public LayerMask capaSuelo;                                      //Define qué capas se consideran "suelo"

    //private CapsuleCollider capsule; //Collider del jugador
    //private float alturaOriginal;    //Valores originales del collider
    //private Vector3 centroOriginal;  //Centro del collider

    private Rigidbody rb;            //Referencia al Rigidbody para movimiento fisico
    private Animator animator;       //Referencia al componente Animator

    void Start()
    {
        rb = GetComponent<Rigidbody>();            //Obtiene el Rigidbody del jugador
        animator = GetComponent<Animator>();       //Obtiene el Animator del jugador
        //capsule = GetComponent<CapsuleCollider>(); //Obtiene el collider del jugador

        ////Guardamos medidas originales del collider
        //alturaOriginal = capsule.height;
        //centroOriginal = capsule.center;

        Cursor.lockState = CursorLockMode.Locked;  //Bloquea el cursor al centro de la pantalla
        Cursor.visible = false;                    //Oculta el cursor durante el juego
    }

    void Update()
    {
        GirarConMouse(); //Control de rotación del jugador con el mouse
        Mover();         //Movimiento horizontal y animaciones de caminar/correr
        Saltar();        //Verifica si puede saltar y aplica la fuerza
        SaltoYCaida();   //Controla animaciones de salto y caida en el aire
        //Agacharse();     //Controla el collider al agacharse 
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal"); //Teclas A/D
        float vertical = Input.GetAxis("Vertical");     //Teclas W/S

        //Dirección basada en la orientacion del personaje
        Vector3 direccion = transform.right * horizontal + transform.forward * vertical;

        //Si se mantiene presionado Shift, usa la velocidad de correr
        float velocidadFinal = Input.GetKey(KeyCode.LeftShift) ? velocidadCorrer : velocidad;

        //Aplica la velocidad a la direccion de movimiento
        Vector3 movimiento = direccion.normalized * velocidadFinal;

        //Aplica movimiento manteniendo la velocidad vertical del Rigidbody
        rb.velocity = new Vector3(movimiento.x, rb.velocity.y, movimiento.z);

        //Calcula la velocidad horizontal para animaciones
        float velocidadActual = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;

        //Actualiza parametros del Animator
        animator.SetFloat("Velocidad", velocidadActual);                   //Para controlar caminar/correr/parar
        animator.SetBool("Corriendo", Input.GetKey(KeyCode.LeftShift));    //Activa animacion de correr si se presiona Shift
    }

    bool EstaEnElSuelo()
    {
        //para detectar colisiones con el suelo
        Collider[] colisiones = Physics.OverlapBox(
            centroDeteccion.position,         //Centro del box
            tamanoDeteccion / 2f,             //Mitad del tamaño 
            Quaternion.identity,              //Sin rotación
            capaSuelo                         //Solo detecta objetos en la capa de suelo
        );

        //Retorna true si se detecta al menos una colisión con el suelo
        return colisiones.Length > 0;
    }

    void Saltar()
    {
        //Si se presiona espacio y esta en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && EstaEnElSuelo())
        {
            //Reinicia la velocidad vertical antes del salto
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Aplica una fuerza hacia arriba como impulso
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void SaltoYCaida()
    {
        //Solo se activa si no esta en el suelo
        if (!EstaEnElSuelo())
        {
            //Si la velocidad "Y" es positiva esta saltando
            if (rb.velocity.y > 0.1f)
            {
                animator.SetBool("Saltando", true);
                animator.SetBool("Cayendo", false);
            }
            //Si la velocidad "Y" es negativa esta cayendo
            else if (rb.velocity.y < -0.1f)
            {
                animator.SetBool("Saltando", false);
                animator.SetBool("Cayendo", true);
            }
        }
        else
        {
            //En el suelo se desactivan las dos
            animator.SetBool("Saltando", false);
            animator.SetBool("Cayendo", false);
        }

    }
    //void Agacharse()
    //{
    //    //Al precionar ctrl
    //    if (Input.GetKey(KeyCode.LeftControl))
    //    {
    //        //Reducir la altura del collider 
    //        capsule.height = alturaOriginal / 1.5f;

    //        //Ajustar centro hacia abajo para que el collider no quede flotando
    //        capsule.center = new Vector3(
    //            centroOriginal.x,
    //            centroOriginal.y - (alturaOriginal - capsule.height) / 2f,
    //            centroOriginal.z
    //        );

    //        //Velocidad mas lenta
    //        velocidad = velocidadAgachado;

    //        //Animacion
    //        animator.SetBool("Agachado", true);
    //    }
    //    else
    //    {
    //        //Restaurar el collider
    //        capsule.height = alturaOriginal;
    //        capsule.center = centroOriginal;

    //        //Restaurar velocidad
    //        velocidad = 5f;

    //        //Animacion
    //        animator.SetBool("Agachado", false);
    //    }
    //}

    void GirarConMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse; //Movimiento horizontal del mouse
        transform.Rotate(Vector3.up * mouseX);                       //Rota al jugador en el eje Y
    }

    void OnDrawGizmosSelected()
    {
        //Dibuja un Gizmo en el editor para visualizar la caja de detección de suelo
        if (centroDeteccion == null) return;

        Gizmos.color = Color.red; //Color rojo para la caja
        Gizmos.DrawWireCube(centroDeteccion.position, tamanoDeteccion); //Dibuja caja del OverlapBox
    }
}
