using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager instancia;

    [Header("UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;

    [System.Serializable]
    public class FraseDialogo
    {
        [TextArea]
        public string texto;

        public bool mostrarContinuar = false; // Ya no se usa Q

        public enum Requisito
        {
            Ninguno,
            DebeMoverse,
            DebeAgarrarObjeto,
            DebeColocarFusible,
            DebeCumplirFusible,
            DebeRepararCables,
            DebeAbrirMisiones,
            DebeCumplirMision1,
            DebeCumplirMision2,
            DebeCumpliEnfermeria,
            DebeCumplirMision3,
            DebeCumplirMision4,
            DebeCumplirCafeteria,
            Fase1, Fase2, Fase3,
        }

        public Requisito requisito = Requisito.Ninguno;
    }

    [Header("Frases del diálogo")]
    public FraseDialogo[] frases;
    private int indice = 0;

    [Header("Velocidad de texto")]
    public float velocidadEscritura = 0.05f;
    private Coroutine escribiendo;
    private bool textoCompleto = false;

    private bool requisitoCumplido = false;
    private FraseDialogo.Requisito requisitoActual = FraseDialogo.Requisito.Ninguno;

    private Dictionary<FraseDialogo.Requisito, bool> requisitosPrevios =
        new Dictionary<FraseDialogo.Requisito, bool>();

    [System.Serializable]
    public class EventoDialogo
    {
        public int indice;
        public UnityEvent evento;
    }

    [Header("Eventos del diálogo")]
    public List<EventoDialogo> eventosDialogo = new List<EventoDialogo>();


    void Awake()
    {
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
        // YA NO SE USA NINGUNA TECLA PARA AVANZAR
    }

    public void IniciarDialogo()
    {
        indice = 0;
        panelDialogo.SetActive(true);
        MostrarSiguienteFrase();
    }

    public void MostrarSiguienteFrase()
    {
        if (indice >= frases.Length)
        {
            CerrarDialogo();
            return;
        }

        if (escribiendo != null)
            StopCoroutine(escribiendo);

        FraseDialogo fraseObj = frases[indice];

        requisitoActual = fraseObj.requisito;

        // Caso 1: No necesita requisito → avanzará automáticamente
        if (requisitoActual == FraseDialogo.Requisito.Ninguno)
        {
            requisitoCumplido = true;
        }
        else if (requisitosPrevios.ContainsKey(requisitoActual))
        {
            // Caso 2: Ya se cumplió antes
            requisitoCumplido = true;
        }
        else
        {
            // Caso 3: Debe esperar a que se cumpla
            requisitoCumplido = false;
        }

        string fraseProcesada = fraseObj.texto.Replace(
            "{NOMBRE}",
            DatosJugador.instancia != null ? DatosJugador.instancia.nombreJugador : "JUGADOR"
        );

        textoDialogo.text = "";
        textoCompleto = false;

        escribiendo = StartCoroutine(EscribirTexto(fraseProcesada));

        EjecutarEventosDialogo(indice);
    }

    IEnumerator EscribirTexto(string frase)
    {
        foreach (char letra in frase)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        textoCompleto = true;

        // Si NO requiere condición → avanzar
        if (requisitoCumplido)
        {
            indice++;
            MostrarSiguienteFrase();
        }
        // Si requiere condición → esperar a CumplirRequisito()
    }

    public void CumplirRequisito(FraseDialogo.Requisito tipo)
    {
        requisitosPrevios[tipo] = true;

        if (tipo == requisitoActual)
        {
            requisitoCumplido = true;

            Debug.Log("Requisito cumplido: " + tipo);

            // Si ya terminó de escribirse → avanzar ahora
            if (textoCompleto)
            {
                indice++;
                MostrarSiguienteFrase();
            }
        }
        else
        {
            Debug.Log("Requisito cumplido por adelantado: " + tipo);
        }
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
        Debug.Log("Dialogo terminado");
    }
}
