using System.Collections.Generic;
using UnityEngine;

public class ControladorBossEva : MonoBehaviour
{
    private EstadoBoss estadoActual;

    // ---------------- FASE 1 ----------------
    private EstadoBossIdleFase1 estadoIdleFase1;
    private EstadoBossSpawn911 estadoSpawn911;
    private EstadoBossTransicionFase2 estadoTransicionFase2;

    [Header("Puntos de aparición 911")]
    public List<PuntoSpawnConVida> puntosSpawn = new List<PuntoSpawnConVida>();

    [Header("Transición Fase 2")]
    public float tiempoTransicion = 3f;

    [Header("Animador")]
    public Animator animador;
    public string parametroEstado = "EstadoBoss";
    public string triggerTransicionFase2 = "TriggerTransicionFase2";

    [Header("IDs Fase 1")]
    public int idIdleFase1 = 0;
    public int idSpawn911 = 1;
    public int idTransicion = 2;

    [Header("Bombas Fase 1")]
    public GameObject prefabBombaPrueba;
    public float intervaloBombasPrueba = 2f;

    // ---------------- FASE 2 ----------------
    [Header("Fase 2 -- Brazos y Zonas")]
    public List<ZonaElectrificada> zonasIzquierda;
    public List<ZonaElectrificada> zonasDerecha;

    public BrazoConVida brazoIzquierdo;
    public BrazoConVida brazoDerecho;

    // Estados
    private EstadoBossIdleFase2 estadoIdleFase2;
    private EstadoBossActivarZonaElectrificada estadoActivarZona;
    private EstadoBossAnimarBrazo estadoAnimarBrazo;
    private EstadoBossBrazoDaño estadoBrazoDaño;
    private EstadoBossBrazoCaido estadoBrazoCaido;
    private EstadoBossTransicionFase3 estadoTransicionFase3;

    // Variables compartidas
    [HideInInspector] public ZonaElectrificada zonaActiva;
    [HideInInspector] public BrazoConVida brazoActual;

    [Header("Animaciones Fase 2")]
    public int idIdleFase2 = 3;
    public int idActivarZona = 4;
    public int idMoverBrazo = 5;
    public int idBrazoDaño = 6;
    public int idBrazoCaido = 7;
    public int idFase3 = 8;

    // ===== TIEMPOS CONFIGURABLES =====
    [Header("Fase 2 — Tiempos Configurables")]
    public float tiempoIdleF2 = 1.5f;
    public float tiempoActivacionZona = 1f;
    public float tiempoAnimacionBrazo = 1.2f;
    public float tiempoVulnerableBrazo = 2f;
    public float tiempoBrazoCaido = 1f;

    [HideInInspector] public bool ultimoAtaqueFueIzquierda = false;

    // ---------- FASE 3 (láseres y torretas) ----------
    [Header("Fase3 - Laser y Torretas")]
    public List<ZonaLaser> zonasLaserSecuenciales = new List<ZonaLaser>();
    public List<ZonaLaser> zonasLaserTodas = new List<ZonaLaser>();
    public float tiempoPorZonaLaser = 1.5f;
    public float tiempoLaserPotente = 2f;

    public float tiempoCansada = 1.0f;
    public List<ControladorTorreta> torretasFase3 = new List<ControladorTorreta>();
    public float rangoTorretasF3 = 12f;
    public float tiempoVulnerableTorretas = 4f;

    [Header("Vida Boss (Fase3)")]
    public VidaBossEva vidaBossEva;
    public int vidaMaximaF3 = 300;
    public int vidaActualF3;

    [Header("Animaciones Fase3 (IDs)")]
    public int idLaserSecuencial = 9;
    public int idLaserPotente = 10;
    public int idTorretasActivas = 11;
    public int idMuerteEva = 12;

    [HideInInspector] public bool puedeRecibirDañoF3 = false;

    // Estados fase3
    private EstadoBossLaserCombinado estadoLaserCombinado;
    private EstadoBossCansadaTorretas estadoCansadaTorretas;
    private EstadoBossMuerte estadoMuerte;

    void Start()
    {
        // Fase 1
        estadoIdleFase1 = new EstadoBossIdleFase1(this);
        estadoSpawn911 = new EstadoBossSpawn911(this);
        estadoTransicionFase2 = new EstadoBossTransicionFase2(this);

        CambiarEstado(estadoIdleFase1);

        // Fase 2
        estadoIdleFase2 = new EstadoBossIdleFase2(this);
        estadoActivarZona = new EstadoBossActivarZonaElectrificada(this);
        estadoAnimarBrazo = new EstadoBossAnimarBrazo(this);
        estadoBrazoDaño = new EstadoBossBrazoDaño(this);
        estadoBrazoCaido = new EstadoBossBrazoCaido(this);
        estadoTransicionFase3 = new EstadoBossTransicionFase3(this);

        // Instancia estados Fase 3
        estadoLaserCombinado = new EstadoBossLaserCombinado(this);
        estadoCansadaTorretas = new EstadoBossCansadaTorretas(this);
        estadoMuerte = new EstadoBossMuerte(this);

        // Inicializar vida F3 y HUD si existe
        vidaActualF3 = vidaMaximaF3;
        if (vidaBossEva != null) vidaBossEva.SetVidaMaxima(vidaMaximaF3);
    }

    void Update()
    {
        if (estadoActual != null)
            estadoActual.ActualizarEstado();
    }

    public void CambiarEstado(EstadoBoss nuevoEstado)
    {
        if (estadoActual != null)
            estadoActual.SalirEstado();

        estadoActual = nuevoEstado;

        if (estadoActual != null)
            estadoActual.EntrarEstado();
    }

    public bool TodosLosPuntosDestruidos()
    {
        foreach (var p in puntosSpawn)
            if (p != null && !p.estaDestruido)
                return false;

        return true;
    }

    // =================== GETTERS ===================
    public EstadoBossIdleFase1 ObtenerIdleFase1() => estadoIdleFase1;
    public EstadoBossSpawn911 ObtenerSpawn911() => estadoSpawn911;
    public EstadoBossTransicionFase2 ObtenerTransicionFase2() => estadoTransicionFase2;

    public EstadoBossIdleFase2 GetEstadoIdleFase2() => estadoIdleFase2;
    public EstadoBossActivarZonaElectrificada GetEstadoActivarZona() => estadoActivarZona;
    public EstadoBossAnimarBrazo GetEstadoAnimarBrazo() => estadoAnimarBrazo;
    public EstadoBossBrazoDaño GetEstadoBrazoDaño() => estadoBrazoDaño;
    public EstadoBossBrazoCaido GetEstadoBrazoCaido() => estadoBrazoCaido;
    public EstadoBossTransicionFase3 GetEstadoTransicionFase3() => estadoTransicionFase3;

    // Getters Fase 3
    public EstadoBossLaserCombinado GetEstadoLaserCombinado() => estadoLaserCombinado;
    public EstadoBossCansadaTorretas GetEstadoCansadaTorretas() => estadoCansadaTorretas;
    public EstadoBossMuerte GetEstadoMuerte() => estadoMuerte;

    // =================== ANIMADOR ===================
    public void SetAnimadorEstado(int valor)
    {
        if (animador != null)
            animador.SetInteger(parametroEstado, valor);
    }

    public void ActivarTriggerTransicionFase2()
    {
        if (animador != null)
            animador.SetTrigger(triggerTransicionFase2);
    }
}
