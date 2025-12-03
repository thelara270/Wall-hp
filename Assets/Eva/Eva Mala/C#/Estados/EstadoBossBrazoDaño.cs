using UnityEngine;

public class EstadoBossBrazoDaño : EstadoBoss
{
    float tiempo;

    public EstadoBossBrazoDaño(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idBrazoDaño);

        // Apagar zona
        if (boss.zonaActiva != null)
            boss.zonaActiva.DesactivarZona();

        // 🔥 Activar daño SOLO en este estado
        if (boss.brazoActual != null)
            boss.brazoActual.ActivarDaño();

        tiempo = boss.tiempoVulnerableBrazo;
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (boss.brazoActual != null && boss.brazoActual.destruido)
        {
            boss.CambiarEstado(boss.GetEstadoBrazoCaido());
            return;
        }

        if (tiempo <= 0)
        {
            // Desactivar daño
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
