using UnityEngine;

public class BolsaArbol : ObjetoInteractuable
{
    [Header("Prefab de planta")]
    public GameObject prefabPlanta; // Prefab que representa la planta a sembrar

    // Devuelve el prefab de la planta asociada
    public GameObject ObtenerPrefab()
    {
        return prefabPlanta;
    }

    // Muestra un mensaje si el jugador intenta usar la bolsa fuera de una zona de siembra
    public override void Usar()
    {
        //UIInventario.Instance.MostrarMensaje("Necesitas estar en una zona de siembra");
    }
}
