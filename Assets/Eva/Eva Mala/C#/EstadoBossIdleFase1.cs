using UnityEngine;

// Estado idle de la fase 1
public class EstadoBossIdleFase1 : EstadoBoss
{
    // Constructor del estado
    public EstadoBossIdleFase1(ControladorBossEva b) : base(b) { }

    // Entrada al estado
    public override void EntrarEstado()
    {
        // Establece el valor del animador al ID de idle fase 1
        boss.SetAnimadorEstado(boss.idIdleFase1);

        // Aquí se podría activar UI que pida "Destruye los puntos de aparición"
    }

    // Actualización del estado
    public override void ActualizarEstado()
    {
        // Inmediatamente transita a spawn para empezar la mecánica
        boss.CambiarEstado(boss.ObtenerSpawn911());
    }

    // Salida del estado
    public override void SalirEstado()
    {
        // No se requiere acción al salir del idle
    }
}
