using UnityEngine;

public class EscenaMusica : MonoBehaviour
{
    public enum TipoMusica
    {
        Menu,
        Cinematica,
        Juego
    }

    public TipoMusica musicaEscena;

    void Start()
    {
        if (AudioManager.instance == null) return;

        switch (musicaEscena)
        {
            case TipoMusica.Menu:
                AudioManager.instance.MusicaMenu();
                break;

            case TipoMusica.Cinematica:
                AudioManager.instance.MusicaCinematica();
                break;

            case TipoMusica.Juego:
                AudioManager.instance.MusicaJuego();
                break;
        }
    }
}
