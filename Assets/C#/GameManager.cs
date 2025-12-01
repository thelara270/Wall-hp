using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string escenaACargar;

    // Panel de pausa asignado desde el Inspector
    public GameObject panelPausa;

    // Variable interna para saber si el juego está pausado
    private bool juegoPausado = false;

    public GameObject canvasHUD;


    private void Update()
    {
        // Detectar si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausarJuego();
        }
    }

    public static void CambiarEscena(string sceneName)
    {
        Time.timeScale = 1f;
        escenaACargar = sceneName;
        SceneManager.LoadScene(escenaACargar);
    }

    public void AbrirPanel(GameObject panelAc)
    {
        panelAc.SetActive(true);
    }

    public void CerrarPanel(GameObject panelCe)
    {
        panelCe.SetActive(false);
    }

    public void PausarJuego()
    {
        if (juegoPausado)
        {
            // Reanudar juego
            panelPausa.SetActive(false);
            canvasHUD.SetActive(true);
            Time.timeScale = 1f;
            juegoPausado = false;

            Cursor.lockState = CursorLockMode.Locked; // Bloquear cursor
            Cursor.visible = false;                   // Ocultar cursor
        }
        else
        {
            // Pausar juego
            panelPausa.SetActive(true);
            canvasHUD.SetActive(false);
            Time.timeScale = 0f;
            juegoPausado = true;

            Cursor.lockState = CursorLockMode.None;   // Liberar cursor
            Cursor.visible = true;                    // Mostrar cursor
        }
    }

    // ⚠️ ESTA FUNCIÓN ES PARA EL BOTÓN "REANUDAR" EN EL MENÚ DE PAUSA
    public void ReanudarDesdeBoton()
    {
        PausarJuego(); // Reutilizamos la misma lógica
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
