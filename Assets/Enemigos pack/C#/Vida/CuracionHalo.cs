using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuracionHalo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ControladorVida vida = other.GetComponent<ControladorVida>();

        if (vida != null)
        {
            vida.ActivarCuracionAutomatica();
            Destroy(gameObject);
        }
    }
}
