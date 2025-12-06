using UnityEngine;
using System.Collections.Generic;

public class EstadoBossActivarZonaElectrificada : EstadoBoss
{
    ZonaElectrificada zonaSeleccionada;
    float tiempo;

    public EstadoBossActivarZonaElectrificada(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        // Animación
        boss.SetAnimadorEstado(boss.idActivarZona);

        bool atacarIzquierda;

        // Si ambos están vivos, alternamos
        if (!boss.brazoIzquierdo.destruido && !boss.brazoDerecho.destruido)
            atacarIzquierda = !boss.ultimoAtaqueFueIzquierda;
        else
            atacarIzquierda = !boss.brazoIzquierdo.destruido;

        // Guardar lado del ataque
        boss.ultimoAtaqueFueIzquierda = atacarIzquierda;

        // Aquí establecemos el brazo actual
        boss.brazoActual = atacarIzquierda ? boss.brazoIzquierdo : boss.brazoDerecho;

        // Seleccionar zona según lado
        List<ZonaElectrificada> lista = atacarIzquierda ?
            boss.zonasIzquierda : boss.zonasDerecha;

        zonaSeleccionada = lista[Random.Range(0, lista.Count)];
        zonaSeleccionada.ActivarZona();

        boss.zonaActiva = zonaSeleccionada;

        tiempo = boss.tiempoActivacionZona;
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
            boss.CambiarEstado(boss.GetEstadoAnimarBrazo());
    }

    public override void SalirEstado() { }
}
