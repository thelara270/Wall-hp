using UnityEngine;

public class EstadoBossAnimarBrazo : EstadoBoss
{
    float tiempo;

    public EstadoBossAnimarBrazo(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idMoverBrazo);

        bool usarIzquierdo = boss.zonaActiva.ladoZona == ZonaElectrificada.LadoZona.Izquierda;

        boss.brazoActual = usarIzquierdo ? boss.brazoIzquierdo : boss.brazoDerecho;

        if (boss.brazoActual.animadorBrazo != null)
            boss.brazoActual.animadorBrazo.SetTrigger(boss.brazoActual.triggerAtacar);

        tiempo = boss.tiempoAnimacionBrazo;
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
            boss.CambiarEstado(boss.GetEstadoBrazoDaño());
    }

    public override void SalirEstado() { }
}
