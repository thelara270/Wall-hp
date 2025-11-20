using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string escenaACargar;
    private GameObject panel;

    public static void CambiarEscena(string sceneName)
    {
        Time.timeScale = 1f;
        escenaACargar = sceneName;
        SceneManager.LoadScene(escenaACargar);
    }

    public void AbrirPanel(GameObject panelAc)
    {
        panel = panelAc;
        panel.SetActive(true);
    }

    public void CerrarPanel(GameObject panelCe)
    {
        panel = panelCe;
        panel.SetActive(false);
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
