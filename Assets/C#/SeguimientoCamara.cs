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
        Vector3 dir = jugador.position - transform.position;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion rotObjetivo = Quaternion.LookRotation(dir);
        Vector3 angulos = rotObjetivo.eulerAngles;

        // 🔹 Normalizar ángulo X a -180 / 180
        float pitch = angulos.x;
        if (pitch > 180) pitch -= 360f;

        float yaw = angulos.y;

        // 🔹 Evitar saltos cuando está demasiado cerca o justo debajo
        if (dir.y > dir.magnitude * 0.9f)
            pitch = rotX;  // No cambiar el pitch si el jugador está exactamente debajo

        // 🔹 Aplicar límites sin bloquear mirar hacia abajo
        pitch = Mathf.Clamp(pitch, limiteVerticalMin, limiteVerticalMax);

        rotX = Mathf.LerpAngle(rotX, pitch, Time.deltaTime * velocidadGiro);
        rotY = Mathf.LerpAngle(rotY, yaw, Time.deltaTime * velocidadGiro);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    void BuscarJugador()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(playerTag);
        if (obj != null)
            jugador = obj.transform;
    }
}