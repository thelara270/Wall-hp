using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaTarjeta : MonoBehaviour
{
    private bool enRango = false;     // Indica si la regadera está dentro del área
    private Tarjeta tarjeta;        // Referencia al componente de la regadera
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tarjeta"))
        {
            enRango = true;
            tarjeta = other.GetComponent<Tarjeta>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tarjeta"))
        {
            enRango = false;
            tarjeta = null;
        }
    }

    private void Update()
    {
        if (enRango && Input.GetKeyDown(KeyCode.E) && tarjeta != null)
        {
            tarjeta.UsarTarjeta("");
        }
    }
}
