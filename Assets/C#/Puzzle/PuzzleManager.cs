using UnityEngine;
using UnityEngine.UI;
using System;

public class PuzzleManager : MonoBehaviour
{
    [Header("Configuración")]
    public PiezaPuzzle[] piezas;        // Todas las piezas del puzzle
    public Button botonReintentar;      // Botón para reiniciar el puzzle

    // Evento público que se dispara cuando el puzzle está completo
    public event Action OnPuzzleCompletado;

    private bool puzzleCompletado = false;

    void Start()
    {
        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(ReiniciarPuzzle);
    }

    void Update()
    {
        if (puzzleCompletado) return;

        bool completo = true;
        foreach (var p in piezas)
        {
            if (p != null && p.enabled)
            {
                completo = false;
                break;
            }
        }

        if (completo)
        {
            puzzleCompletado = true;
            Debug.Log("¡Puzzle completado!");

            // Dispara el evento
            OnPuzzleCompletado?.Invoke();
        }
    }

    public void ReiniciarPuzzle()
    {
        puzzleCompletado = false;

        foreach (var pieza in piezas)
        {
            if (pieza == null) continue;

            pieza.enabled = true;
            pieza.ReiniciarEstado();
        }

        Debug.Log("Puzzle reiniciado correctamente.");
    }
}
