using UnityEngine;

public class EstadoBossBrazoCaido : EstadoBoss
{
    float tiempo;

    public EstadoBossBrazoCaido(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idBrazoCaido);

        if (boss.zonaActiva != null)
            boss.zonaActiva.DesactivarZona();

        boss.brazoActual.DesactivarDaño();

        tiempo = boss.tiempoBrazoCaido;
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
        {
            bool quedaIzq = !boss.brazoIzquierdo.destruido;
            bool quedaDer = !boss.brazoDerecho.destruido;

            if (quedaIzq || quedaDer)
                boss.CambiarEstado(boss.GetEstadoIdleFase2());
            else
                boss.CambiarEstado(boss.GetEstadoTransicionFase3());
        }
    }

    public override void SalirEstado() { }
}
