using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensajePanel : MonoBehaviour
{
    public GameObject Mensaje;

    private void Start()
    {
        Mensaje.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Mensaje.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Mensaje.SetActive(false);
    }
}
