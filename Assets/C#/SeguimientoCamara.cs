using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    public string playerTag = "Player";
    private Transform jugador;

    [Header("Velocidad de rotación")]
    public float velocidadGiro = 4f;

    [Header("Límites de inclinación (vertical)")]
    public float limiteVerticalMin = -20f; // mirar hacia abajo
    public float limiteVerticalMax = 45f;  // mirar hacia arriba

    private float rotX; // Pitch
    private float rotY; // Yaw

    void Start()
    {
        BuscarJugador();

        // inicializar rotaciones
        Vector3 e = transform.eulerAngles;
        rotX = e.x;
        rotY = e.y;
    }

    void Update()
    {
        if (jugador == null)
        {
            BuscarJugador();
            return;
        }

        SeguirJugador();
    }

    void SeguirJugador()
    {
        // Direccion hacia el jugador
        Vector3 dir = jugador.position - transform.position;

        if (dir.sqrMagnitude < 0.001f)
            return;

        // Convertir a rotación objetivo
        Quaternion rotObjetivo = Quaternion.LookRotation(dir);
        Vector3 angulos = rotObjetivo.eulerAngles;

        // Normalizar para valores de -180 a 180
        float pitch = angulos.x;
        if (pitch > 180) pitch -= 360f;

        float yaw = angulos.y;

        // Aplicar límites al pitch (eje X)
        pitch = Mathf.Clamp(pitch, limiteVerticalMin, limiteVerticalMax);

        // Rotación suave
        rotX = Mathf.Lerp(rotX, pitch, Time.deltaTime * velocidadGiro);
        rotY = Mathf.Lerp(rotY, yaw, Time.deltaTime * velocidadGiro);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    void BuscarJugador()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(playerTag);
        if (obj != null)
            jugador = obj.transform;
    }
}