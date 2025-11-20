using UnityEngine;

public class MisionSala1 : MisionBase
{
    // Variables de progreso de la misión
    public bool fusiblesColocados = false;        // Indica si los fusibles fueron colocados
    public bool generadorReparado = false;        // Indica si el generador fue reparado
    public bool panelElectricoReparado = false;   // Indica si el panel fue reparado

    private void Start()
    {
        // Inicia automáticamente la misión al cargar la escena
        IniciarMision();
    }

    private void Update()
    {
        // Solo verifica el progreso si la misión está en curso
        if (estado == EstadoMision.EnProgreso)
            VerificarProgreso();
    }

    public override void VerificarProgreso()
    {
        // Comprueba si todas las tareas necesarias están completas
        if (fusiblesColocados && generadorReparado && panelElectricoReparado)
            CompletarMision();
    }

    // Marca los fusibles como colocados
    public void ColocarFusibles()
    {
        fusiblesColocados = true;
        Debug.Log("Fusibles colocados");

        // Simula que al colocar los fusibles se repara el generador
        RepararGenerador();
    }

    // Marca el generador como reparado
    private void RepararGenerador()
    {
        generadorReparado = true;
        Debug.Log("Generador reparado al colocar los fusibles");
    }

    // Marca el panel eléctrico como reparado
    public void RepararPanel()
    {
        panelElectricoReparado = true;
        Debug.Log("Panel eléctrico reparado");
    }
}
