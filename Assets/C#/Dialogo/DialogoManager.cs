using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager instancia; // acceso global rápido

    [Header("UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;

    // -----------------------------
    //   SISTEMA DE FRASES NUEVO
    // -----------------------------
    [System.Serializable]
    public class FraseDialogo
    {
        [TextArea]
        public string texto;

        [Tooltip("Si está activado, agrega automáticamente 'Presiona Enter para continuar.'")]
        public bool mostrarContinuar = true;

        public enum Requisito
        {
            Ninguno,
            DebeMoverse,
            DebeSaltar,
            DebeAgarrarObjeto,
            DebeColocarFusible,
            DebeRepararCables
        }

        [Tooltip("Requisito que debe cumplirse antes de permitir avanzar desde esta frase.")]
        public Requisito requisito = Requisito.Ninguno;
    }

    [Header("Frases del diálogo")]
    public FraseDialogo[] frases;
    private int indice = 0;

    // -----------------------------
    [Header("Velocidad de texto")]
    public float velocidadEscritura = 0.05f;
    private Coroutine escribiendo;
    private bool textoCompleto = false;

    // Control de requisito actual
    private bool requisitoCumplido = false;
    private FraseDialogo.Requisito requisitoActual = FraseDialogo.Requisito.Ninguno;

    // -----------------------------
    [System.Serializable]
    public class EventoDialogo
    {
        [Tooltip("Índice del diálogo que activará este evento.")]
        public int indice;
        public UnityEvent evento;
    }

    [Header("Eventos del diálogo")]
    public List<EventoDialogo> eventosDialogo = new List<EventoDialogo>();

    void Awake()
    {
        // Singleton simple
        if (instancia == null) instancia = this;
        else if (instancia != this) Destroy(gameObject);
    }

    void Start()
    {
        panelDialogo.SetActive(true);
        IniciarDialogo();
    }

    void Update()
    {
        if (!panelDialogo.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!textoCompleto)
            {
                // Completar el texto inmediatamente
                CompletarTextoActual();
            }
            else
            {
                // Si hay un requisito para esta frase y no se cumplió → no permitir avanzar
                if (requisitoActual != FraseDialogo.Requisito.Ninguno && !requisitoCumplido)
                {
                    // Opcional: reproducir un sonido o mostrar un UI que indique "requisito no cumplido"
                    Debug.Log("⛔ Requisito no cumplido: " + requisitoActual);
                    return;
                }

                // reset del flag para la siguiente frase (se volverá a asignar en MostrarSiguienteFrase)
                requisitoCumplido = false;

                // Pasar al siguiente diálogo
                MostrarSiguienteFrase();
            }
        }
    }

    public void IniciarDialogo()
    {
        indice = 0;
        panelDialogo.SetActive(true);
        MostrarSiguienteFrase();
    }

    public void MostrarSiguienteFrase()
    {
        if (indice < frases.Length)
        {
            if (escribiendo != null)
                StopCoroutine(escribiendo);

            // Obtener la frase actual
            FraseDialogo fraseObj = frases[indice];

            // Registrar requisito actual y por defecto marcar cumplido si no requiere nada
            requisitoActual = fraseObj.requisito;
            requisitoCumplido = (requisitoActual == FraseDialogo.Requisito.Ninguno);

            // Reemplazar nombre del jugador
            string fraseProcesada = fraseObj.texto.Replace("{NOMBRE}", DatosJugador.instancia != null ? DatosJugador.instancia.nombreJugador : "JUGADOR");

            // Agregar el texto de "Enter para continuar" si está activado
            if (fraseObj.mostrarContinuar)
                fraseProcesada += "\n\nPresiona Enter para continuar.";

            textoDialogo.text = "";
            textoCompleto = false;

            escribiendo = StartCoroutine(EscribirTexto(fraseProcesada));

            EjecutarEventosDialogo(indice);
        }
        else
        {
            CerrarDialogo();
        }
    }

    IEnumerator EscribirTexto(string frase)
    {
        foreach (char letra in frase)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        textoCompleto = true;
        indice++; // Avanza automáticamente al final de la escritura (índice apunta al siguiente)
    }

    void CompletarTextoActual()
    {
        if (escribiendo != null)
            StopCoroutine(escribiendo);

        // Nota: cuando se completó, el índice apunta al siguiente (porque EscribirTexto incrementa al terminar).
        // Para obtener la frase actual mostramos el elemento índice - 1 (con clamp para seguridad).
        FraseDialogo fraseObj = frases[Mathf.Clamp(indice, 0, frases.Length - 1)];
        // Si texCompleto==false y EscribirTexto no alcanzó a incrementar, usamos Clamp(indice,0,len-1).
        // Esta línea es compatible con la estructura actual.

        string fraseProcesada = fraseObj.texto.Replace("{NOMBRE}", DatosJugador.instancia != null ? DatosJugador.instancia.nombreJugador : "JUGADOR");

        if (fraseObj.mostrarContinuar)
            fraseProcesada += "\n\nPresiona Enter para continuar.";

        textoDialogo.text = fraseProcesada;
        textoCompleto = true;

        indice++;
    }

    // Llamar desde otros scripts para marcar el requisito como cumplido
    public void CumplirRequisito()
    {
        requisitoCumplido = true;
        Debug.Log("✅ Requisito cumplido: " + requisitoActual);
    }

    void EjecutarEventosDialogo(int indiceActual)
    {
        foreach (var evento in eventosDialogo)
        {
            if (evento.indice == indiceActual)
            {
                evento.evento?.Invoke();
            }
        }
    }

    void CerrarDialogo()
    {
        if (escribiendo != null)
            StopCoroutine(escribiendo);

        panelDialogo.SetActive(false);
        Debug.Log("📖 Diálogo terminado");
    }
}
