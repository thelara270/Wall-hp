using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilJP : MonoBehaviour
{
    public int daño;

    private void OnTriggerEnter(Collider other)
    {
        ControladorVida vida = other.GetComponent<ControladorVida>();

        if (vida != null)
        {
            vida.RecibirDaño(daño);
        }
    }
}
