using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DañoEnemigo : MonoBehaviour
{
    public int daño = 20; // Daño por golpe o bala

    private void OnCollisionEnter(Collision collision)
    {
        ControladorVidaEnemigo vida = collision.gameObject.GetComponent<ControladorVidaEnemigo>();

        if (vida != null)
        {
            vida.RecibirDaño(daño);
        }
    }

    // También funciona con triggers
    private void OnTriggerEnter(Collider other)
    {
        ControladorVidaEnemigo vida = other.GetComponent<ControladorVidaEnemigo>();

        if (vida != null)
        {
            vida.RecibirDaño(daño);
        }
    }
}