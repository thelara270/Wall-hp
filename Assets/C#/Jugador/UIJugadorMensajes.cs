using TMPro;
using UnityEngine;

public class UIJugadorMensajes : MonoBehaviour
{
    public static UIJugadorMensajes Instance;

    [Header("UI de mensajes")]
    public TextMeshProUGUI mensajeUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //Muestra mensaje al jugador
    public void MostrarMensaje(string mensaje)
    {
        if (mensajeUI != null)
        {
            mensajeUI.text = mensaje;
        }
    }

    //Limpia el texto
    public void LimpiarMensaje()
    {
        if (mensajeUI != null)
        {
            mensajeUI.text = "";
        }
    }
}
