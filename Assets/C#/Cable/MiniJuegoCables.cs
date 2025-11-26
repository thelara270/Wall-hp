using UnityEngine;

public class MinijuegoCables : MonoBehaviour
{
    [Header("Configuracion del minijuego")]
    public int totalConexiones = 3;        //Numero de cables que deben conectarse
    private int conexionesCorrectas;   //Numero de cables que ya fueron conectados correctamente

    [Header("UI")]
    public GameObject uiJugador;       //Canvas de la UI del jugador
    public GameObject uiMinijuego;     //Canvas de la UI del minijuego

    public PanelElectrico panel;       //Referencia al panel electrico
    [SerializeField] MisionSala1 mision; //Referencia a la mision

    private bool estadoPanel;    //Boleano para no abrir el panel

    private void Start()
    {
        conexionesCorrectas = 0;
        estadoPanel = false;
    }

    public void IniciarMinijuego()
    {
        if (estadoPanel == false)
        {
            //Libera el cursor y lo vuelve visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Oculta la UI del jugador y activa la del minijuego
            uiJugador.SetActive(false);
            uiMinijuego.SetActive(true);

            //Reinicia el conteo de conexiones correctas
            conexionesCorrectas = 0;
        }
    }

    // Llamar desde la lógica del minijuego cuando se conecta correctamente un cable
    public void CompletarConexion()
    {
        conexionesCorrectas++; //Suma la conexion realizada
        Debug.Log("Conexiones correctas: " + conexionesCorrectas + "/" + totalConexiones);

        //Si ya se conectaron todos los cables
        if (conexionesCorrectas >= totalConexiones)
        {
            Debug.Log("Todos los cables conectados. El Panel ha sido reparado");

            estadoPanel = true;

            AudioManager.instance?.SonidoPanelCarpetas();

            //Cerrar el minijuego y volver a la UI del jugador
            uiJugador.SetActive(true);
            uiMinijuego.SetActive(false);

            //Actualiza mision como progreso completado
            if (mision != null) mision.panelElectricoReparado = true;

            //Avisar al panel electrico que fue reparado
            if (panel != null) panel.Reparar();

            // Notificar al DialogoManager que completaste el minijuego
            DialogoManager.instancia?.CumplirRequisito();
        }
    }
}
