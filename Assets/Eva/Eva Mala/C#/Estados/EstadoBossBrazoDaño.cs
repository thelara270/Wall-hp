using UnityEngine;

public class EstadoBossBrazoDaño : EstadoBoss
{
    float tiempo;

    public EstadoBossBrazoDaño(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idBrazoDaño);

        if (boss.zonaActiva != null)
            boss.zonaActiva.DesactivarZona();

        if (boss.brazoActual != null)
            boss.brazoActual.ActivarDaño();

        tiempo = boss.tiempoVulnerableBrazo;
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        // ✔ Solo si está destruido
        if (boss.brazoActual != null && boss.brazoActual.destruido)
        {
            boss.CambiarEstado(boss.GetEstadoBrazoCaido());
            return;
        }

        // Si sigue vivo → volver a Idle Fase 2
        if (tiempo <= 0)
        {
            if (boss.brazoActual != null)
                boss.brazoActual.DesactivarDaño();

            boss.CambiarEstado(boss.GetEstadoIdleFase2());
        }
    }

    public override void SalirEstado()
    {
        if (boss.brazoActual != null)
            boss.brazoActual.DesactivarDaño();
    }
}
