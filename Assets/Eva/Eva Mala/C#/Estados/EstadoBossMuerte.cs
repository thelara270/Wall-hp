using UnityEngine;

// EstadoBossMuerte: rutina de muerte del boss.
public class EstadoBossMuerte : EstadoBoss
{
    float tiempo = 2f;

    public EstadoBossMuerte(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        // Animación de muerte
        boss.SetAnimadorEstado(boss.idMuerteEva);

        // Desactivar cualquier zona laser activa
        if (boss.zonasLaserSecuenciales != null)
            foreach (var z in boss.zonasLaserSecuenciales) z.DesactivarZona();

        if (boss.zonasLaserTodas != null)
            foreach (var z in boss.zonasLaserTodas) z.DesactivarZona();

        // Desactivar torretas (dejar en idle y restaurar rangos si aplica)
        if (boss.torretasFase3 != null)
        {
            foreach (var torreta in boss.torretasFase3)
            {
                if (torreta == null) continue;
                torreta.ChangeState(torreta.GetIdleState());
            }
        }

        // Evitar que reciba más daño
        boss.puedeRecibirDañoF3 = false;
        if (boss.vidaBossEva != null) boss.vidaBossEva.DesactivarDaño();

        Debug.Log("EVA: Estado de muerte iniciado.");
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0f)
        {
            // Aquí podrías disparar la eliminación del boss, recompensa, cambio de escena, etc.
            Debug.Log("EVA: muerte finalizada. Implementa lógica de final de combate aquí.");
        }
    }

    public override void SalirEstado()
    {
        // No hace nada al salir
    }
}
