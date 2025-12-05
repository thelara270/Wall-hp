using System.Collections;
using UnityEngine;

public class ZonaCodigo : MonoBehaviour
{
    public GameObject panel;
    public GameObject uiJugador;

    public float tiempoCierreAutomatico = 2f;
    [HideInInspector] public bool bloqueoSalida = false;

    private bool enRango;
    private bool ignoreNextE = false;

    public GameManager gameManager;

    private void Update()
    {
        ActivarPanel();
        DesactivarPanel();
    }

    public void ActivarPanel()
    {
        if (enRango && Input.GetKeyDown(KeyCode.E) && !panel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            bloqueoSalida = false;
            ignoreNextE = true;

            uiJugador.SetActive(false);
            panel.SetActive(true);

            StartCoroutine(BloquearEscapeUnFrame());
        }
    }

    private IEnumerator BloquearEscapeUnFrame()
    {
        gameManager.bloqueoPausa = true;
        yield return null;
    }

    public void DesactivarPanel()
    {
        if (!panel.activeSelf)
            return;

        if (ignoreNextE)
        {
            ignoreNextE = false;
            return;
        }

        if (bloqueoSalida)
            return;

        if (Input.GetKeyDown(KeyCode.E))
            CerrarPanel();
    }

    public void CerrarPanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        uiJugador.SetActive(true);
        panel.SetActive(false);

        gameManager.bloqueoPausa = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enRango = true;
            Debug.Log("Presiona E para reparar el panel electrico");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enRango = false;
    }
}
