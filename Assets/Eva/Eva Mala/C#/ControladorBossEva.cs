using System.Collections.Generic;
using UnityEngine;

// Controlador principal del boss
public class ControladorBossEva : MonoBehaviour
{
    // Referencia al estado actual del boss
    private EstadoBoss estadoActual;

    // Estado Idle de la fase 1
    private EstadoBossIdleFase1 estadoIdleFase1;
    // Estado de spawn 911 de la fase 1
    private EstadoBossSpawn911 estadoSpawn911;
    // Estado de transición a fase 2
    private EstadoBossTransicionFase2 estadoTransicionFase2;

    // Lista de puntos de aparición 911
    [Header("Puntos de aparición 911")]
    public List<PuntoSpawnConVida> puntosSpawn = new List<PuntoSpawnConVida>();

    // Tiempo de la transición entre fases
    [Header("Transición Fase 2")]
    public float tiempoTransicion = 3f;

    // Referencia al Animator del boss
    [Header("Animador")]
    public Animator animador;

    // Nombre del parámetro entero que indica el estado/animación principal
    [Tooltip("Parámetro entero usado para cambiar el estado visual del boss")]
    public string parametroEstado = "EstadoBoss";

    // Nombre del trigger que activa la cinemática/transición a fase 2
    [Tooltip("Trigger que dispara la transición a la fase 2 en el animador")]
    public string triggerTransicionFase2 = "TriggerTransicionFase2";

    // IDs de animación para control por entero
    [Header("IDs de animación")]
    public int idIdleFase1 = 0;
    public int idSpawn911 = 1;
    public int idTransicion = 2;

    [Header("Pruebas de bombas Fase 1")]
    public GameObject prefabBombaPrueba;
    public float intervaloBombasPrueba = 2f;

    // Inicio del controlador
    void Start()
    {
        // Instancia los estados de la fase 1
        estadoIdleFase1 = new EstadoBossIdleFase1(this);
        estadoSpawn911 = new EstadoBossSpawn911(this);
        estadoTransicionFase2 = new EstadoBossTransicionFase2(this);

        // Inicia en el estado idle de la fase 1
        CambiarEstado(estadoIdleFase1);
    }

    // Actualización por frame
    void Update()
    {
        // Llama al update del estado actual si existe
        if (estadoActual != null)
            estadoActual.ActualizarEstado();
    }

    // Cambia el estado actual del boss
    public void CambiarEstado(EstadoBoss nuevoEstado)
    {
        // Si hay un estado actual, ejecuta su salida
        if (estadoActual != null)
            estadoActual.SalirEstado();

        // Asigna el nuevo estado
        estadoActual = nuevoEstado;

        // Si el nuevo estado existe, ejecuta su entrada
        if (estadoActual != null)
            estadoActual.EntrarEstado();
    }

    // Devuelve true si todos los puntos de spawn están destruidos
    public bool TodosLosPuntosDestruidos()
    {
        // Recorre cada punto en la lista
        foreach (var p in puntosSpawn)
        {
            // Si existe el punto y no está destruido, retorna false
            if (p != null && !p.estaDestruido)
                return false;
        }

        // Si ninguno devolvió false, todos están destruidos
        return true;
    }

    // Devuelve el estado Idle fase 1
    public EstadoBossIdleFase1 ObtenerIdleFase1() { return estadoIdleFase1; }
    // Devuelve el estado Spawn 911
    public EstadoBossSpawn911 ObtenerSpawn911() { return estadoSpawn911; }
    // Devuelve el estado Transición fase 2
    public EstadoBossTransicionFase2 ObtenerTransicionFase2() { return estadoTransicionFase2; }

    // Método auxiliar para cambiar el parámetro entero del animator
    public void SetAnimadorEstado(int valor)
    {
        // Comprueba que el animador esté asignado
        if (animador != null)
            animador.SetInteger(parametroEstado, valor);
    }

    // Método auxiliar para disparar un trigger en el animator
    public void ActivarTriggerTransicionFase2()
    {
        // Comprueba que el animador esté asignado
        if (animador != null)
            animador.SetTrigger(triggerTransicionFase2);
    }
}
