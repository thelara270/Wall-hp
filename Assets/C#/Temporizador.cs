using UnityEngine;
using TMPro;

public class Temporizador : MonoBehaviour
{
    [Header("Configuración del tiempo")]
    [Tooltip("Duración del temporizador en segundos")]
    public float tiempoTotal = 120f; // por defecto 2 minutos

    [Header("UI")]
    [Tooltip("Referencia al texto donde se mostrará el tiempo restante")]
    public TextMeshProUGUI textoTemporizador;

    [Header("Acción al finalizar")]
    [Tooltip("Objeto que se activará al finalizar el tiempo")]
    public GameObject objetoActivar;
    public GameObject hud;

    public float tiempoRestante;
    private bool enMarcha = true;

    void Start()
    {
        tiempoRestante = tiempoTotal;
        if (objetoActivar != null)
            objetoActivar.SetActive(false);
    }

    void Update()
    {
        if (enMarcha)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                enMarcha = false;
                FinTemporizador();
            }

            MostrarTiempo();
        }
    }

    void MostrarTiempo()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTemporizador.text = $"{minutos:00}:{segundos:00}";
    }

    void FinTemporizador()
    {
        hud.SetActive(false);
        Debug.Log("⏰ ¡Tiempo terminado!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        if (objetoActivar != null)
            objetoActivar.SetActive(true);
    }
}
