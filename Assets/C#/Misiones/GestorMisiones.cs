using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class GestorMisiones : MonoBehaviour
{
    // Instancia estática para acceder al gestor desde otros scripts
    public static GestorMisiones Instance;

    [Header("Misiones activas")]
    public List<MisionBase> misionesActivas = new List<MisionBase>();

    [Header("Eventos globales de misión")]
    public UnityEvent<MisionBase> onCualquierMisionCompletada; // UnityEvent con parámetro de tipo MisionBase

    // Evento tradicional por código (C#)
    public event Action<MisionBase> OnMisionCompletada;

    private void Awake()
    {
        // Garantiza que solo haya una instancia del gestor
        if (Instance == null)
            Instance = this;
    }

    // Activa una misión si aún no está activa
    public void ActivarMision(MisionBase mision)
    {
        // Si la misión no está ya en la lista de activas
        if (!misionesActivas.Contains(mision))
        {
            // Inicia la misión
            mision.IniciarMision();

            // La agrega a la lista de misiones activas
            misionesActivas.Add(mision);
        }
    }

    // Notifica que una misión fue completada
    public void NotificarMisionCompletada(MisionBase mision)
    {
        Debug.Log("Gestor recibió misión completada: " + mision.nombreMision);

        // Elimina la misión de la lista de activas
        misionesActivas.Remove(mision);

        // Llama al evento de C#
        OnMisionCompletada?.Invoke(mision);

        // Llama al UnityEvent (puede configurarse desde el inspector)
        onCualquierMisionCompletada?.Invoke(mision);
    }
}
