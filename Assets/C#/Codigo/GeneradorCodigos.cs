using UnityEngine;
using TMPro;
using System;

// Clase que genera, muestra y verifica códigos aleatorios para una misión.
public class GeneradorCodigos : MonoBehaviour
{
    [Header("Configuración")]
    public int cantidadCaracteres = 6;       // Longitud de cada código generado.
    public TMP_Text[] panelesCodigo;         // Referencias a los textos que muestran los códigos.
    public TMP_InputField inputJugador;      // Campo de texto donde el jugador escribe el código.
    public TMP_Text resultadoTexto;          // Texto que muestra el resultado de la verificación.

    private string codigoCorrecto;           // Almacena el código correcto actual.
    private int indiceCorrecto;              // Índice del panel que contiene el código correcto.
    private System.Random random;            // Generador de números aleatorios.

    // Evento que se dispara cuando el jugador ingresa correctamente el código.
    public event Action OnCodigoCorrecto;

    private void Start()
    {
        // Inicializa el generador de números aleatorios.
        random = new System.Random();

        // Genera los códigos poco después de iniciar el juego.
        Invoke(nameof(GenerarCodigos), 0.1f);
    }

    // Genera un conjunto de códigos, colocando uno verdadero y los demás falsos.
    public void GenerarCodigos()
    {
        // Verifica que haya paneles asignados en el inspector.
        if (panelesCodigo == null || panelesCodigo.Length == 0)
        {
            Debug.LogError("No hay paneles asignados en el Inspector.");
            return;
        }

        // Genera el código correcto y elige aleatoriamente en qué panel colocarlo.
        codigoCorrecto = GenerarCodigoAleatorio(cantidadCaracteres);
        indiceCorrecto = random.Next(0, panelesCodigo.Length);

        // Asigna códigos a cada panel, colocando el verdadero solo en uno.
        for (int i = 0; i < panelesCodigo.Length; i++)
        {
            if (i == indiceCorrecto)
                panelesCodigo[i].text = codigoCorrecto;
            else
                panelesCodigo[i].text = GenerarCodigoAleatorio(cantidadCaracteres);
        }

        // Limpia el texto del resultado anterior.
        resultadoTexto.text = "";
    }

    // Genera un código aleatorio de letras y números en mayúsculas.
    private string GenerarCodigoAleatorio(int longitud)
    {
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string codigo = "";

        for (int i = 0; i < longitud; i++)
        {
            int index = random.Next(0, caracteres.Length);
            codigo += caracteres[index];
        }

        return codigo;
    }

    // Se ejecuta cuando el jugador presiona el botón "Verificar".
    public void VerificarCodigo()
    {
        // Convierte el texto del jugador a mayúsculas y lo compara con el código correcto.
        if (inputJugador.text.ToUpper() == codigoCorrecto)
        {
            resultadoTexto.text = "Código correcto.";
            resultadoTexto.color = Color.green;

            // Dispara el evento solo en el momento correcto.
            OnCodigoCorrecto?.Invoke();
        }
        else
        {
            resultadoTexto.text = "Código incorrecto.";
            resultadoTexto.color = Color.red;
        }
    }

    // Devuelve true si el jugador ha escrito el código correcto.
    public bool CodigoEsCorrecto()
    {
        return inputJugador.text.ToUpper() == codigoCorrecto;
    }
}
