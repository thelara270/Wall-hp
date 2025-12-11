using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camaraJugador;
    public Transform puntoDisparo;
    public CamaraTercera camaraTercera;

    [Header("Mano")]
    public Transform manoJugador;

    [Header("Disparo")]
    public float cadencia = 0.15f;
    private float tiempoDisparo = 0f;

    [Header("Pool")]
    public GameObject balaPrefab;
    public int cantidadPool = 20;
    private Queue<GameObject> poolBalas = new Queue<GameObject>();

    [Header("Efectos Disparo")]
    public AudioClip sonidoDisparo;
    public ParticleSystem particulasDisparo;


    void Start()
    {
        IntentarObtenerReferencias();
        CrearPool();
    }

    void Update()
    {
        // Si faltan referencias, seguir intentando hasta encontrarlas
        if (!ReferenciasListas())
        {
            IntentarObtenerReferencias();
            return;
        }

        // No dispara si no está en la mano del jugador
        if (transform.parent != manoJugador)
            return;

        if (Input.GetMouseButton(0) && Time.time > tiempoDisparo)
        {
            if (camaraTercera != null && camaraTercera.apuntando)   // ← SOLO dispara si apunta
            {
                Disparar();
                tiempoDisparo = Time.time + cadencia;
            }
        }
    }

    // -------------------------------------------------------------
    // ------------------ SISTEMA DE REFERENCIAS -------------------
    // -------------------------------------------------------------

    bool ReferenciasListas()
    {
        return camaraJugador != null &&
               camaraTercera != null &&
               manoJugador != null &&
               puntoDisparo != null;
    }

    void IntentarObtenerReferencias()
    {
        if (camaraJugador == null)
        {
            GameObject camara = GameObject.FindGameObjectWithTag("MainCamera");
            if (camara != null)
                camaraJugador = camara.GetComponent<Camera>();
        }

        if (camaraTercera == null && camaraJugador != null)
        {
            camaraTercera = camaraJugador.GetComponent<CamaraTercera>();
        }

        if (manoJugador == null)
        {
            GameObject mano = GameObject.FindGameObjectWithTag("Mano");
            if (mano != null)
                manoJugador = mano.transform;
        }
    }

    // -------------------------------------------------------------
    // ---------------------- SISTEMA DE POOL -----------------------
    // -------------------------------------------------------------

    void CrearPool()
    {
        for (int i = 0; i < cantidadPool; i++)
        {
            GameObject nuevaBala = Instantiate(balaPrefab);
            nuevaBala.SetActive(false);
            poolBalas.Enqueue(nuevaBala);
        }
    }

    GameObject ObtenerBala()
    {
        GameObject bala = poolBalas.Dequeue();
        bala.SetActive(true);
        return bala;
    }

    public void RetornarBala(GameObject bala)
    {
        bala.SetActive(false);
        poolBalas.Enqueue(bala);
    }

    // -------------------------------------------------------------
    // --------------------------- DISPARO --------------------------
    // -------------------------------------------------------------

    void Disparar()
    {


        GameObject bala = ObtenerBala();
        bala.transform.position = puntoDisparo.position;

        if (particulasDisparo != null)
            particulasDisparo.Play();
        AudioManager.instance.PlaySFX(sonidoDisparo);

        Vector3 direccion;

        // Si hay enemigo targeteado
        if (camaraTercera != null && camaraTercera.enemigoMasCercano != null)
        {
            direccion = (camaraTercera.enemigoMasCercano.position - puntoDisparo.position).normalized;
        }
        else
        {
            // Raycast desde la cámara
            if (Physics.Raycast(camaraJugador.transform.position,
                                camaraJugador.transform.forward,
                                out RaycastHit hit, 200f))
            {
                direccion = (hit.point - puntoDisparo.position).normalized;
            }
            else
            {
                direccion = camaraJugador.transform.forward;
            }
        }

        Bala b = bala.GetComponent<Bala>();
        b.Disparar(direccion, this);
    }
}
