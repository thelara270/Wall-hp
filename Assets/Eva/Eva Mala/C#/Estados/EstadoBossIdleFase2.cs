using UnityEngine;

public class EstadoBossIdleFase2 : EstadoBoss
{
    float tiempoIdle;

    public EstadoBossIdleFase2(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idIdleFase2);
        tiempoIdle = boss.tiempoIdleF2;
    }

    public override void ActualizarEstado()
    {
        tiempoIdle -= Time.deltaTime;

        if (tiempoIdle <= 0)
            boss.CambiarEstado(boss.GetEstadoActivarZona());
    }

    public override void SalirEstado() { }
}
