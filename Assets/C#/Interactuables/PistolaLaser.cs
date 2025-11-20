using UnityEngine;

public class PistolaLaser : ObjetoInteractuable
{
    [Header("Ajustes de Disparo")]
    public float daño = 10f;                  //Daño que hace el láser
    public float alcance = 50f;               //Distancia máxima del disparo
    public float tiempoEntreDisparos = 0.3f;  //Retraso entre disparos
    public Transform puntoDisparo;            //Desde dónde sale el láser (punta de la pistola)
    public ParticleSystem efectoDisparo;      //Efecto visual del disparo (opcional)
    public LineRenderer lineaLaser;           //Línea visual del láser

    [Header("Apuntar")]
    public float zoomApuntado = 2f;           //Factor de zoom al apuntar
    public float velocidadZoom = 5f;          //Qué tan rápido hace el zoom
    private bool apuntando = false;           //Estado de apuntado

    private Camera camaraJugador;             //Referencia a la cámara del jugador
    private float campoVisionOriginal;        //Campo de visión original de la cámara
    private float tiempoUltimoDisparo;        //Controla el intervalo entre disparos

    void Start()
    {
        //Obtiene la cámara principal
        camaraJugador = Camera.main;

        //Guarda el campo de visión original
        campoVisionOriginal = camaraJugador.fieldOfView;

        //Desactiva el LineRenderer al inicio
        if (lineaLaser != null)
            lineaLaser.enabled = false;
    }

    void Update()
    {
        //Si no está equipada, no hace nada
        if (transform.parent == null) return;

        //Detecta si el jugador mantiene clic derecho (botón secundario)
        //if (Input.GetMouseButton(1))
        //{
        //    apuntando = true;
        //}
        //else
        //{
        //    apuntando = false;
        //}

        //Cambia el zoom de la cámara gradualmente
        float objetivoFOV = apuntando ? campoVisionOriginal / zoomApuntado : campoVisionOriginal;
        camaraJugador.fieldOfView = Mathf.Lerp(
            camaraJugador.fieldOfView,
            objetivoFOV,
            Time.deltaTime * velocidadZoom
        );

        //Dispara con clic izquierdo (botón principal)
        if (Input.GetMouseButton(0) && Time.time >= tiempoUltimoDisparo + tiempoEntreDisparos)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
        }
    }

    void Disparar()
    {
        //Activa efecto de partículas si hay
        if (efectoDisparo != null)
            efectoDisparo.Play();

        //Calcula el origen y la dirección del disparo
        Transform origen = puntoDisparo != null ? puntoDisparo : transform;
        Vector3 direccion = origen.transform.right;

        //Raycast para detectar colisión
        if (Physics.Raycast(origen.position, direccion, out RaycastHit hit, alcance))
        {
            //Si impacta algo, muestra el láser hasta ese punto
            MostrarLaser(origen.position, hit.point);

            //Intenta aplicar daño si el objeto tiene un componente "Salud"
            //Salud objetivo = hit.collider.GetComponent<Salud>();
            //if (objetivo != null)
            //{
            //    objetivo.RecibirDaño(daño);
            //}
        }
        else
        {
            //Si no impacta nada, muestra el láser hasta el máximo alcance
            MostrarLaser(origen.position, origen.position + direccion * alcance);
        }
    }

    void MostrarLaser(Vector3 inicio, Vector3 fin)
    {
        //Si no hay línea, salimos
        if (lineaLaser == null) return;

        //Activa el LineRenderer y configura los puntos
        lineaLaser.enabled = true;
        lineaLaser.SetPosition(0, inicio);
        lineaLaser.SetPosition(1, fin);

        //Desactiva la línea después de un corto tiempo
        Invoke(nameof(DesactivarLaser), 1f);
    }

    void DesactivarLaser()
    {
        //Desactiva el efecto visual del láser
        if (lineaLaser != null)
            lineaLaser.enabled = false;
    }

    public override void Usar()
    {
        //Sobrescribe el método "Usar" para evitar mensaje de texto
        //UIInventario.Instance.MostrarMensaje("Pistola lista para disparar");
    }
}
