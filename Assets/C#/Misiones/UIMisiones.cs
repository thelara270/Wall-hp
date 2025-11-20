using UnityEngine;

public class UIMisiones : MonoBehaviour
{
    // Referencia al objeto del HUD (interfaz principal del jugador)
    public GameObject hud;

    // Referencia al panel de misiones o menú que se mostrará al presionar TAB
    public GameObject panelMisiones;

    // Variable que indica si el juego está en pausa o no
    private bool juegoPausado = false;

    [Header("Botones de misiones")]
    public UIMisionIndividual[] botonesMisiones;

    private void Start()
    {
        panelMisiones.SetActive(false);
        hud.SetActive(true);
    }

    void Update()
    {
        // Verifica si el jugador presionó la tecla TAB en este cuadro de actualización
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Si el juego está pausado, lo reanuda
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            // Si el juego no está pausado, lo pausa
            else
            {
                PausarJuego();
            }
        }
    }

    // Método que pausa el juego y muestra el panel de misiones
    void PausarJuego()
    {
        panelMisiones.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f;
        juegoPausado = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Actualiza todos los botones de misión al abrir el panel
        foreach (var boton in botonesMisiones)
            boton.ActualizarVisual();
    }

    // Método que reanuda el juego y oculta el panel de misiones
    void ReanudarJuego()
    {
        // Desactiva el panel de misiones
        panelMisiones.SetActive(false);

        // Activa nuevamente el HUD
        hud.SetActive(true);

        // Restaura el tiempo del juego a la velocidad normal
        Time.timeScale = 1f;

        // Marca que el juego ya no está pausado
        juegoPausado = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
