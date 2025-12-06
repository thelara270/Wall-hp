using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tarjeta : ObjetoInteractuable
{
    public string escenaACargar;

    public void UsarTarjeta(string sceneName)
    {
        SceneManager.LoadScene(escenaACargar);
    }
}
