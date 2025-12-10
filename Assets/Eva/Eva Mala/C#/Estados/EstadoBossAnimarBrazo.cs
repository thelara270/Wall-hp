using UnityEngine;

public class EstadoBossAnimarBrazo : EstadoBoss
{
    float tiempo;

    public EstadoBossAnimarBrazo(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        // Usa el brazo que YA fue elegido en EstadoBossActivarZonaElectrificada
        bool usarIzquierdo = boss.brazoActual == boss.brazoIzquierdo;

        if (usarIzquierdo)
            boss.SetAnimadorEstado(boss.idAtacarIzquierda);
        else
            boss.SetAnimadorEstado(boss.idAtacarDerecha);

        tiempo = boss.tiempoAnimacionBrazo;
        AudioManager.instance.SonidoPegarEva();

    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
            boss.CambiarEstado(boss.GetEstadoBrazoDaño());
    }

    public override void SalirEstado() { }
}
