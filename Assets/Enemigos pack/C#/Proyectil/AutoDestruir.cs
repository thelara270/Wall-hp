using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruir : MonoBehaviour
{
    public float tiempoVida = 2f; // Ajusta según la duración de la partícula

    private void OnEnable()
    {
        Destroy(gameObject, tiempoVida);
    }
}
