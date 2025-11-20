using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CambiarCinematica : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // Asigna tu VideoPlayer en el Inspector
    public string nombreEscenaDestino;   // Escena a la que quieres ir

    void Start()
    {
        // Escuchar el evento cuando el video termina
        videoPlayer.loopPointReached += AlTerminarVideo;
    }

    void AlTerminarVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(nombreEscenaDestino);
    }
}
