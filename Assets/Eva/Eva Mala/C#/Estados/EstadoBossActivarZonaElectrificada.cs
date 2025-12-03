using UnityEngine;
using System.Collections.Generic;

public class EstadoBossActivarZonaElectrificada : EstadoBoss
{
    ZonaElectrificada zonaSeleccionada;
    float tiempo;

    public EstadoBossActivarZonaElectrificada(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idActivarZona);

        bool atacarIzquierda;

        if (!boss.brazoIzquierdo.destruido && !boss.brazoDerecho.destruido)
        {
            atacarIzquierda = !boss.ultimoAtaqueFueIzquierda;
        }
        else
        {
            atacarIzquierda = !boss.brazoIzquierdo.destruido;
        }

        boss.ultimoAtaqueFueIzquierda = atacarIzquierda;

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
