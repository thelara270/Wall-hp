using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventario : MonoBehaviour
{
    public static UIInventario Instance; // Instancia estática para acceder fácilmente desde otros scripts

    [Header("Referencias UI")]
    public Image[] iconosSlots;         // Slots del inventario
    public Image iconoRecogerUI;        // Ícono de interacción (por ejemplo, para recoger objetos)

    [SerializeField] private Animator animador; // Controlador de animaciones del inventario

    private void Awake()
    {
        Instance = this;

        // Asegura que el ícono de recoger esté desactivado al inicio
        if (iconoRecogerUI != null)
            iconoRecogerUI.enabled = false;
    }

    /// <summary>
    /// Muestra u oculta el ícono de recoger/interactuar
    /// y activa o desactiva los iconos de los slots temporalmente.
    /// </summary>
    public void MostrarIconoRecoger(Sprite icono, bool mostrar)
    {
        if (iconoRecogerUI == null) return;

        // Solo controla el ícono principal de recoger
        iconoRecogerUI.sprite = icono;
        iconoRecogerUI.enabled = mostrar;
        iconoRecogerUI.gameObject.SetActive(mostrar);


        // Si quieres que el inventario se vea al mismo tiempo (sin cambiar sprites):
        if (mostrar)
            MostrarTemporalmente();
    }

    /// <summary>
    /// Actualiza los íconos del inventario según el estado actual de los objetos.
    /// </summary>
    public void ActualizarSlots(ObjetoInteractuable[] slots, int slotActivo)
    {
        for (int i = 0; i < iconosSlots.Length; i++)
        {
            if (slots[i] != null)
            {
                // Muestra el ícono del objeto y resalta el slot activo
                iconosSlots[i].sprite = slots[i].icono;
                iconosSlots[i].color = (i == slotActivo) ? Color.gray : Color.white;
                iconosSlots[i].gameObject.SetActive(true);
            }
            else
            {
                // Si el slot está vacío, se hace más transparente
                iconosSlots[i].sprite = null;
                iconosSlots[i].color = (i == slotActivo) ? Color.gray : new Color(1, 1, 1, 0.3f);
                iconosSlots[i].gameObject.SetActive(true);
            }
        }

        // Muestra temporalmente el inventario
        MostrarTemporalmente();
    }

    /// <summary>
    /// Hace visible el inventario por unos segundos.
    /// </summary>
    public void MostrarTemporalmente()
    {
        if (animador == null) return;

        animador.SetBool("Visible", true);
        CancelInvoke(nameof(OcultarInventario));
        Invoke(nameof(OcultarInventario), 1.5f);
    }

    /// <summary>
    /// Oculta el inventario mediante animación.
    /// </summary>
    public void OcultarInventario()
    {
        if (animador == null) return;
        animador.SetBool("Visible", false);
    }
}
