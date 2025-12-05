using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CamaraTercera : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;

    [Header("Distancia")]
    public float distancia = 4f;
    public float distanciaMin = 1f;
    public float distanciaMax = 4f;

    [Header("Zoom al Apuntar")]
    public float distanciaAIM = 2f;        // 🔥 Nuevo: Cámara más cerca al apuntar
    public float velocidadZoom = 10f;      // 🔥 Nuevo: Suavidad de zoom

    [Header("Altura")]
    public float altura = 2f;

    [Header("Rotación")]
    public float sensibilidadX = 120f;
    public float sensibilidadY = 120f;
    public float minY = -40f;
    public float maxY = 60f;

    [Header("Auto Apuntado")]
    public float rangoAutoAim = 15f;       // 🔥 Nuevo
    public LayerMask capaEnemigos;         // 🔥 Nuevo
    public Transform enemigoMasCercano;    // 🔥 Nuevo

    [Header("UI")]
    public GameObject miraUIPrefab;   // el prefab
    private GameObject miraUI;        
    public Canvas canvasJugador;

    [Header("Apuntar")]
    public bool apuntando = false;
    public Vector3 offsetNormal = new Vector3(0f, 0f, 0f);
    public Vector3 offsetApuntar = new Vector3(0.5f, 0f, 0f); // → mueve la cámara a la derecha del personaje
    public float distanciaApuntar = 2.5f; // distancia cuando apunta
    public float suavidadCambioModo = 10f;

    [Header("Suavidad")]
    public float suavidadMovimiento = 10f;
    public float suavidadRotacion = 15f;

    [Header("Colisiones")]
    public LayerMask capasColision;

    private float rotX = 0f;
    private float rotY = 0f;
    private float distanciaActual;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        distanciaActual = distancia;

        // ---------------------------------------
        // 🔥 CREAR CANVAS SI NO EXISTE
        // ---------------------------------------
        if (canvasJugador == null)
        {
            GameObject nuevoCanvas = new GameObject("CanvasMira");
            canvasJugador = nuevoCanvas.AddComponent<Canvas>();
            canvasJugador.renderMode = RenderMode.ScreenSpaceOverlay;

            nuevoCanvas.AddComponent<CanvasScaler>();
            nuevoCanvas.AddComponent<GraphicRaycaster>();

            DontDestroyOnLoad(nuevoCanvas);
        }

        // ---------------------------------------
        // 🔥 INSTANCIAR LA MIRA DENTRO DEL CANVAS
        // ---------------------------------------
        if (miraUIPrefab != null)
        {
            miraUI = Instantiate(miraUIPrefab, canvasJugador.transform);
            miraUI.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        //DetectarEnemigoMasCercano();

        if (Input.GetMouseButtonDown(1)) apuntando = true;
        if (Input.GetMouseButtonUp(1)) apuntando = false;

        if (Input.GetMouseButtonDown(1))
            apuntando = true;

        if (Input.GetMouseButtonUp(1))
            apuntando = false;

        RotarCamara();

        if (apuntando && objetivo != null)
        {
            Vector3 forward = transform.forward;
            forward.y = 0; // evitar inclinación rara
            objetivo.rotation = Quaternion.Lerp(
                objetivo.rotation,
                Quaternion.LookRotation(forward),
                Time.deltaTime * 10f
            );
        }

        CalcularDistanciaConColision();
        MoverCamara();

        ActualizarUI();
    }

    // ------------------------------------------------------
    //       BUSCAR EL ENEMIGO MÁS CERCA
    // ------------------------------------------------------
    //void DetectarEnemigoMasCercano()
    //{
    //    Collider[] detectados = Physics.OverlapSphere(objetivo.position, rangoAutoAim, capaEnemigos);

    //    if (detectados.Length == 0)
    //    {
    //        enemigoMasCercano = null;
    //        return;
    //    }

    //    enemigoMasCercano = detectados
    //        .OrderBy(x => Vector3.Distance(objetivo.position, x.transform.position))
    //        .First()
    //        .transform;
    //}

    // ------------------------------------------------------
    //      ROTACIÓN DE CÁMARA + AUTO AIM
    // ------------------------------------------------------
    void RotarCamara()
    {
        // Rotación libre con el mouse
        rotX += Input.GetAxis("Mouse X") * sensibilidadX * Time.deltaTime;
        rotY -= Input.GetAxis("Mouse Y") * sensibilidadY * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, minY, maxY);

        //// SI SE ESTÁ APUNTANDO Y HAY ENEMIGO: GIRAR HACIA ÉL
        //if (apuntando && enemigoMasCercano != null)
        //{
        //    Vector3 direccion = enemigoMasCercano.position - objetivo.position;
        //    Quaternion rotObjetivo = Quaternion.LookRotation(direccion);

        //    // Convertir a ángulos para la cámara
        //    Vector3 ang = rotObjetivo.eulerAngles;

        //    rotX = Mathf.Lerp(rotX, ang.y, Time.deltaTime * 5f);
        //}
    }

    // ------------------------------------------------------
    //      COLISIONES DE CÁMARA CON ZOOM AL APUNTAR
    // ------------------------------------------------------
    void CalcularDistanciaConColision()
    {
        float distanciaDeseada = apuntando ? distanciaAIM : distancia;

        Vector3 origen = objetivo.position + Vector3.up * altura;

        Quaternion rot = Quaternion.Euler(rotY, rotX, 0);
        Vector3 destinoDeseado = objetivo.position - rot * Vector3.forward * distanciaDeseada + Vector3.up * altura;

        Vector3 direccion = destinoDeseado - origen;

        if (Physics.Raycast(origen, direccion.normalized, out RaycastHit hit, distanciaDeseada, capasColision))
        {
            distanciaActual = Mathf.Clamp(hit.distance - 0.3f, distanciaMin, distanciaMax);
        }
        else
        {
            distanciaActual = Mathf.Lerp(distanciaActual, distanciaDeseada, Time.deltaTime * velocidadZoom);
        }
    }

    // ------------------------------------------------------
    //                  MOVER LA CÁMARA
    // ------------------------------------------------------
    void MoverCamara()
    {
        Quaternion rot = Quaternion.Euler(rotY, rotX, 0);

        float distanciaObjetivo = apuntando ? distanciaApuntar : distanciaActual;
        Vector3 offsetObjetivo = apuntando ? offsetApuntar : offsetNormal;

        Vector3 posDeseada =
            objetivo.position
            - rot * Vector3.forward * distanciaObjetivo
            + Vector3.up * altura
            + rot * offsetObjetivo;   // ← el offset se aplica en dirección de la cámara

        transform.position = Vector3.Lerp(
            transform.position,
            posDeseada,
            suavidadCambioModo * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            rot,
            suavidadRotacion * Time.deltaTime
        );
    }

    // ------------------------------------------------------
    //                  UI de la MIRA
    // ------------------------------------------------------
    void ActualizarUI()
    {
        if (miraUI != null)
            miraUI.SetActive(apuntando);
    }
}