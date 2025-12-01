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
        FuenteDePoder
    }

    [Header("Configuración")]
    public TipoImagen tipoImagen;

    [Header("Referencias")]
    public Image imagenUI;              // La imagen UI que aparecerá
    public Sprite fusible;
    public Sprite electrico;
    public Sprite codigo;
    public Sprite ponerCodigo;
    public Sprite fuenteDePoder;

    private void Start()
    {
        // Asegura que empiece invisible
        SetAlpha(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cambia sprite según el enum
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
                imagenUI.sprite = fusible;
                break;
            case TipoImagen.FuenteDePoder:
                imagenUI.sprite = fusible;
                break;
        }

        // La muestra (alpha = 1)
        SetAlpha(1f);
    }

    private void OnTriggerExit(Collider other)
    {
        // La oculta con alpha = 0
        SetAlpha(0f);
    }

    private void SetAlpha(float a)
    {
        Color c = imagenUI.color;
        c.a = a;
        imagenUI.color = c;
    }
}
