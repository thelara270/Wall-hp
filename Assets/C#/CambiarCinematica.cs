using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CambiarCinematica : MonoBehaviour
{
    public VideoPlayer videoPlayer;            // Asignar VideoPlayer
    public string nombreEscenaDestino;         // Escena destino

    public DesvanecerPantalla fade;            // Script de Fade (el tuyo)
    public float fadeAntesDeFinal = 2f;        // Cuándo iniciar fade OUT antes del final

    private bool fadeOutActivado = false;

    void Start()
    {
        // FADE IN al comenzar la escena / cinemática
        fade.IniciarFadeIn(0f); // Puedes cambiar el tiempo de espera

        // Evento al terminar el video
        videoPlayer.loopPointReached += AlTerminarVideo;
    }

    void Update()
    {
        if (!videoPlayer.isPrepared) return;

        // Calcular cuánto falta para el final
        double tiempoRestante = videoPlayer.length - videoPlayer.time;

        // Iniciar fade OUT antes de que termine el video
        if (!fadeOutActivado && tiempoRestante <= fadeAntesDeFinal)
        {
            fadeOutActivado = true;
            fade.IniciarFadeOut();   // 👈 Aquí llamamos tu fade out
        }
    }

    void AlTerminarVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}
