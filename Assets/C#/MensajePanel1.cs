using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensajePanelGanaste : MonoBehaviour
{
    public GameObject panel;
    public GameObject panelHud;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        panelHud.SetActive(false);
        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
