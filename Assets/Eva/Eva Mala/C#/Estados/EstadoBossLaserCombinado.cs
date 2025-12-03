using UnityEngine;
using System.Collections.Generic;

// EstadoBossLaserCombinado: activa láseres secuenciales y luego todos juntos (potente).
public class EstadoBossLaserCombinado : EstadoBoss
{
    // índice en la secuencia
    int indice = 0;
    // temporizador interno
    float tiempo = 0f;
    // fase interna: 0 = secuencial, 1 = potente, 2 = fin
    int faseInterna = 0;

    public EstadoBossLaserCombinado(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        // Animación secuencial de láser
        boss.SetAnimadorEstado(boss.idLaserSecuencial);

        // Reset de variables
        indice = 0;
        faseInterna = 0;
        tiempo = 0f;

        // Si no hay zonas definidas, salta inmediatamente
        if (boss.zonasLaserSecuenciales == null || boss.zonasLaserSecuenciales.Count == 0)
        {
            faseInterna = 1;
            tiempo = boss.tiempoLaserPotente;
            // iniciar potente si hay zonas definidas para potente
            if (boss.zonasLaserTodas != null)
            {
                foreach (var z in boss.zonasLaserTodas)
                    z.DesactivarZona();
            }
            return;
        }

        // Asegura que todas las zonas estén desactivadas al entrar
        if (boss.zonasLaserSecuenciales != null)
            foreach (var z in boss.zonasLaserSecuenciales) z.DesactivarZona();

        if (boss.zonasLaserTodas != null)
            foreach (var z in boss.zonasLaserTodas) z.DesactivarZona();

        // comienza la secuencia activando la primera si existe
        if (boss.zonasLaserSecuenciales.Count > 0)
        {
            boss.zonasLaserSecuenciales[0].ActivarZona();
            tiempo = boss.tiempoPorZonaLaser;
            indice = 0;
            faseInterna = 0;
        }
        else
        {
            faseInterna = 1;
            tiempo = boss.tiempoLaserPotente;
        }
    }

    public override void ActualizarEstado()
    {
        if (faseInterna == 0)
        {
            // Secuencial: mantener una zona activa por tiempoPorZonaLaser, luego avanzar
            tiempo -= Time.deltaTime;

            if (tiempo <= 0f)
            {
                // Desactivar la zona actual
                if (indice >= 0 && indice < boss.zonasLaserSecuenciales.Count)
                    boss.zonasLaserSecuenciales[indice].DesactivarZona();

                // Avanzar
                indice++;

                if (indice < boss.zonasLaserSecuenciales.Count)
                {
                    // Activar siguiente y reiniciar tiempo
                    boss.zonasLaserSecuenciales[indice].ActivarZona();
                    tiempo = boss.tiempoPorZonaLaser;
                }
                else
                {
                    // Secuencia terminada → pasar a potente
                    faseInterna = 1;
                    tiempo = boss.tiempoLaserPotente;

                    // Activar todas las zonas para el ataque potente
                    if (boss.zonasLaserTodas != null)
                    {
                        foreach (var z in boss.zonasLaserTodas)
                        {
                            z.ActivarZona();
                        }
                    }

                    // Cambiar animación a potente
                    boss.SetAnimadorEstado(boss.idLaserPotente);
                }
            }
        }
        else if (faseInterna == 1)
        {
            // Fase potente: todas las zonas activas por tiempoLaserPotente
            tiempo -= Time.deltaTime;

            if (tiempo <= 0f)
            {
                // Desactivar todas las zonas potentes
                if (boss.zonasLaserTodas != null)
                {
                    foreach (var z in boss.zonasLaserTodas)
                        z.DesactivarZona();
                }

                // Transicionar a estado cansada/torretas
                if (boss.GetEstadoCansadaTorretas() != null)
                    boss.CambiarEstado(boss.GetEstadoCansadaTorretas());
                else
                    Debug.LogWarning("EstadoBossCansadaTorretas no definido en ControladorBossEva.");
            }
        }
    }

    public override void SalirEstado()
    {
        // asegurar que todas las zonas estén apagadas
        if (boss.zonasLaserSecuenciales != null)
            foreach (var z in boss.zonasLaserSecuenciales) z.DesactivarZona();

        if (boss.zonasLaserTodas != null)
            foreach (var z in boss.zonasLaserTodas) z.DesactivarZona();
    }
}
