using UnityEngine;
using static DialogoManager;

public class UIMisiones : MonoBehaviour
{
    public GameObject hud;
    public GameObject panelMisiones;

    private bool juegoPausado = false;

    public GameManager gameManager;

    [Header("Botones de misiones")]
    public UIMisionIndividual[] botonesMisiones;

    private void Start()
    {
        panelMisiones.SetActive(false);
        hud.SetActive(true);
    }

    void Update()
    {
        // 🔒 NO permitir abrir si algo bloquea la pausa
        if (gameManager.bloqueoPausa && !juegoPausado)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (juegoPausado)
                ReanudarJuego();
            else
                PausarJuego();
        }
    }

    void PausarJuego()
    {
        panelMisiones.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        juegoPausado = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DialogoManager.instancia?.CumplirRequisito(FraseDialogo.Requisito.DebeAbrirMisiones);

        // Bloquear sistema de pausa global
        gameManager.bloqueoPausa = true;

        foreach (var boton in botonesMisiones)
            boton.ActualizarVisual();
    }

    void ReanudarJuego()
    {
        panelMisiones.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f;
        juegoPausado = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Desbloquear sistema de pausa global
        gameManager.bloqueoPausa = false;
    }
}
