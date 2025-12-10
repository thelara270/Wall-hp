using UnityEngine;
using UnityEngine.UI;

public class MensajePanel : MonoBehaviour
{
    public enum TipoImagen
    {
        Fusible,
        Electrico,
        Codigo,
        PonerCodigo,
        FuenteDePoder,
        AbrirPuerta,
        PuertaBloqueada,   // <- nombre corregido y consistente
    }

    [Header("Configuración")]
    public TipoImagen tipoImagen;

    [Header("Referencias UI")]
    public Image imagenUI;
    public Sprite fusible;
    public Sprite electrico;
    public Sprite codigo;
    public Sprite ponerCodigo;
    public Sprite fuenteDePoder;
    public Sprite abrirPuerta;
    public Sprite puertaBloqueada; // <- nombre consistente con enum

    [Header("Opcional")]
    public Puertas puerta;   // Referencia opcional a la puerta (para comprobar si está bloqueada)

    private void Start()
    {
        SetAlpha(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si este MensajePanel está ligado a una puerta, priorizamos el estado de la puerta
        if ((tipoImagen == TipoImagen.AbrirPuerta || tipoImagen == TipoImagen.PuertaBloqueada) && puerta != null)
        {
            if (puerta.bloqueada)
                imagenUI.sprite = puertaBloqueada;
            else
                imagenUI.sprite = abrirPuerta;
        }
        else
        {
            // Caso normal: elegir sprite según el enum
            switch (tipoImagen)
            {
                case TipoImagen.Fusible:
                    imagenUI.sprite = fusible;
                    break;
                case TipoImagen.Electrico:
                    imagenUI.sprite = electrico;
                    break;
                case TipoImagen.Codigo:
                    imagenUI.sprite = codigo;
                    break;
                case TipoImagen.PonerCodigo:
                    imagenUI.sprite = ponerCodigo;
                    break;
                case TipoImagen.FuenteDePoder:
                    imagenUI.sprite = fuenteDePoder;
                    break;
                case TipoImagen.AbrirPuerta:
                    // Si no hay referencia a la puerta, asumimos que mostramos abrirPuerta
                    imagenUI.sprite = abrirPuerta;
                    break;
                case TipoImagen.PuertaBloqueada:
                    imagenUI.sprite = puertaBloqueada;
                    break;
                default:
                    Debug.LogWarning($"TipoImagen no manejado: {tipoImagen}");
                    break;
            }
        }

        SetAlpha(1f);
    }

    private void OnTriggerExit(Collider other)
    {
        SetAlpha(0f);
    }

    private void SetAlpha(float a)
    {
        if (imagenUI == null) return;
        Color c = imagenUI.color;
        c.a = a;
        imagenUI.color = c;
    }
}
