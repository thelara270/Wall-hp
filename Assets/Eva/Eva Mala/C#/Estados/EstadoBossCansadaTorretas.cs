using UnityEngine;
using System.Collections.Generic;

// EstadoBossCansadaTorretas: Eva queda cansada, activa torretas y se vuelve vulnerable.
public class EstadoBossCansadaTorretas : EstadoBoss
{
    // temporizador de vulnerabilidad
    float tiempo;

    // almacenamiento de rangos previos de torretas para restaurar
    private List<float> rangosPrevios = new List<float>();

    public EstadoBossCansadaTorretas(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        // Animación de cansancio / torretas activas
        boss.SetAnimadorEstado(boss.idTorretasActivas);

        // Temporizador de vulnerabilidad
        tiempo = boss.tiempoVulnerableTorretas;

        // Permitir que EVA reciba daño en fase 3
        boss.puedeRecibirDañoF3 = true;
        if (boss.vidaBossEva != null) boss.vidaBossEva.ActivarDaño();

        // Activar torretas y ajustar rango; guardar valores previos
        rangosPrevios.Clear();
        if (boss.torretasFase3 != null)
        {
            foreach (var torreta in boss.torretasFase3)
            {
                if (torreta == null) continue;
                // guardar rango previo
                rangosPrevios.Add(torreta.rangoDeteccion);
                // asignar nuevo rango desde el boss
                torreta.rangoDeteccion = boss.rangoTorretasF3;
                // forzar al estado de detección para que empiecen a disparar si ven al jugador
                torreta.ChangeState(torreta.GetDetectState());
            }
        }

        // Asegurar que las zonas laser estén apagadas (por si quedaron)
        if (boss.zonasLaserSecuenciales != null)
            foreach (var z in boss.zonasLaserSecuenciales) z.DesactivarZona();

        if (boss.zonasLaserTodas != null)
            foreach (var z in boss.zonasLaserTodas) z.DesactivarZona();
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        // Si EVA murió por balas durante este tiempo
        if (boss.vidaBossEva != null && boss.vidaBossEva.destruida)
        {
            boss.CambiarEstado(boss.GetEstadoMuerte());
            return;
        }

        if (tiempo <= 0f)
        {
            // Termina vulnerabilidad: desactivar posibilidad de recibir daño
            boss.puedeRecibirDañoF3 = false;
            if (boss.vidaBossEva != null) boss.vidaBossEva.DesactivarDaño();

            // Restaurar rangos previos y poner torretas en idle
            if (boss.torretasFase3 != null)
            {
                int i = 0;
                foreach (var torreta in boss.torretasFase3)
                {
                    if (torreta == null) continue;
                    // restaurar rango si tenemos valor previo
                    if (i < rangosPrevios.Count)
                        torreta.rangoDeteccion = rangosPrevios[i];
                    i++;
                    // volver a estado idle para que no detecten forzosamente
                    torreta.ChangeState(torreta.GetIdleState());
                }
            }

            // Si aún vive, volver a ciclo de láseres
            if (boss.vidaActualF3 > 0)
                boss.CambiarEstado(boss.GetEstadoLaserCombinado());
            else
                boss.CambiarEstado(boss.GetEstadoMuerte());
        }
    }

    public override void SalirEstado()
    {
        // seguridad: desactivar daño y restaurar torretas si algo falló
        boss.puedeRecibirDañoF3 = false;
        if (boss.vidaBossEva != null) boss.vidaBossEva.DesactivarDaño();

        if (boss.torretasFase3 != null)
        {
            int i = 0;
            foreach (var torreta in boss.torretasFase3)
            {
                if (torreta == null) continue;
                if (i < rangosPrevios.Count)
                    torreta.rangoDeteccion = rangosPrevios[i];
                i++;
                torreta.ChangeState(torreta.GetIdleState());
            }
        }
    }
}
