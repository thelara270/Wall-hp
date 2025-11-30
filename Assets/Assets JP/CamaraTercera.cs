using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamaraTercera : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;

    [Header("Distancia")]
    public float distancia = 4f;
    public float distanciaMin = 1f;
    public float distanciaMax = 4f;

    [Header("Altura")]
    public float altura = 2f;

    [Header("Rotación")]
    public float sensibilidadX = 120f;
    public float sensibilidadY = 120f;
    public float minY = -40f;
    public float maxY = 60f;

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
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        RotarCamara();
        CalcularDistanciaConColision();
        MoverCamara();
    }

    void RotarCamara()
    {
        rotX += Input.GetAxis("Mouse X") * sensibilidadX * Time.deltaTime;
        rotY -= Input.GetAxis("Mouse Y") * sensibilidadY * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, minY, maxY);
    }

    void CalcularDistanciaConColision()
    {
        // Punto desde donde se lanza el raycast (desde la cabeza del jugador)
        Vector3 origen = objetivo.position + Vector3.up * altura;

        // Direccion aproximada hacia donde queremos que esté la cámara
        Quaternion rot = Quaternion.Euler(rotY, rotX, 0);
        Vector3 destinoDeseado = objetivo.position - rot * Vector3.forward * distancia + Vector3.up * altura;

        // Dirección desde el personaje hacia la cámara
        Vector3 direccion = destinoDeseado - origen;

        // Distancia hasta donde se esperaría colocar la cámara
        float distanciaObjetivo = distancia;

        // Raycast
        if (Physics.Raycast(origen, direccion.normalized, out RaycastHit hit, distancia, capasColision))
        {
            // Si pega contra algo, reduce la distancia
            distanciaActual = Mathf.Clamp(hit.distance - 0.3f, distanciaMin, distanciaMax);
        }
        else
        {
            // Si NO pega, vuelve a la distancia normal
            distanciaActual = distanciaObjetivo;
        }
    }

    void MoverCamara()
    {
        Quaternion rot = Quaternion.Euler(rotY, rotX, 0);

        Vector3 posDeseada =
            objetivo.position
            - rot * Vector3.forward * distanciaActual
            + Vector3.up * altura;

        transform.position = Vector3.Lerp(transform.position, posDeseada, suavidadMovimiento * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, suavidadRotacion * Time.deltaTime);
    } 
}