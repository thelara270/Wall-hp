using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curacion : MonoBehaviour
{
    public int cantidadCuracion;

    private void OnTriggerEnter(Collider other)
    {
        ControladorVida vida = other.GetComponent<ControladorVida>();

        if (vida != null)
        {
            vida.Curarse(cantidadCuracion);
            Destroy(gameObject);
        }
    }
}
