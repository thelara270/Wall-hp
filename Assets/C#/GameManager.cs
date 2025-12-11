using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string escenaACargar;

    public GameObject panelPausa;
    public GameObject canvasHUD;

    public bool bloqueoPausa = false;

    private bool juegoPausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !bloqueoPausa)
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
        if (bloqueoPausa)
            return;

        if (juegoPausado)
        {
            panelPausa.SetActive(false);
            canvasHUD.SetActive(true);
            Time.timeScale = 1f;
            juegoPausado = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            panelPausa.SetActive(true);
            canvasHUD.SetActive(false);
            Time.timeScale = 0f;
            juegoPausado = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ReanudarDesdeBoton()
    {
        PausarJuego();
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }

    public void MostrarMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
