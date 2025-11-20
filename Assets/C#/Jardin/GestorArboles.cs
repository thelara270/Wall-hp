using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GestorArboles : MonoBehaviour
{
    // Instancia estática
    public static GestorArboles Instance;

    [Header("UI de progreso")]
    public Image barraProgreso;      // Imagen tipo Fill para mostrar progreso
    public int totalArboles = 3;     // Cantidad total de árboles necesarios

    private List<CrecimientoArbol> arboles = new List<CrecimientoArbol>(); // Lista de árboles registrados
    private int arbolesCompletos = 0; // Contador de árboles listos

    private void Start()
    {
        // Inicializa la barra en vacío
        arbolesCompletos = 0;
        if (barraProgreso != null)
            barraProgreso.fillAmount = 0f;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Registra un nuevo árbol en la lista
    public void RegistrarArbol(CrecimientoArbol nuevoArbol)
    {
        if (!arboles.Contains(nuevoArbol))
        {
            arboles.Add(nuevoArbol);
            nuevoArbol.OnArbolListo += ContarArbolCompleto;
        }
    }

    // Se ejecuta cuando un árbol termina su crecimiento
    private void ContarArbolCompleto(CrecimientoArbol arbol)
    {
        arbolesCompletos++;
        ActualizarBarra();

        // Si todos los árboles están listos, completa la misión de Sala 2
        if (arbolesCompletos >= totalArboles)
        {
            MisionSala2 mision = FindObjectOfType<MisionSala2>();
            if (mision != null)
                mision.CompletarMision();
        }
    }

    // Actualiza el valor de la barra de progreso
    private void ActualizarBarra()
    {
        if (barraProgreso != null)
            barraProgreso.fillAmount = (float)arbolesCompletos / totalArboles;
    }
}
