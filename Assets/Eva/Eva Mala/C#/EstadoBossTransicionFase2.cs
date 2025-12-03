using UnityEngine;

// Estado de transición entre fase 1 y fase 2
public class EstadoBossTransicionFase2 : EstadoBoss
{
    // Temporizador restante de la transición
    private float tiempoRestante;

    // Constructor del estado
    public EstadoBossTransicionFase2(ControladorBossEva b) : base(b) { }

    // Entrada al estado
    public override void EntrarEstado()
    {
        // Inicializa el temporizador con el valor del boss
        tiempoRestante = boss.tiempoTransicion;

        // Cambia el parámetro entero del animador al ID de transición
        boss.SetAnimadorEstado(boss.idTransicion);

        // Dispara el trigger específico para la cinemática/transición
        boss.ActivarTriggerTransicionFase2();

        // Aquí podrías iniciar efectos visuales globales (oscurecer, vibración, sonido)
    }

    // Actualización del estado
    public override void ActualizarEstado()
    {
        // Reduce el temporizador
        tiempoRestante -= Time.deltaTime;

        // Si el tiempo terminó
        if (tiempoRestante <= 0f)
        {
            // Log para depuración de la transición
            Debug.Log("Transición a Fase 2 completada");

            // En este punto se debería cambiar al primer estado de la Fase 2
            // Por ahora no hay estado Fase 2 implementado, así que dejamos la lógica aquí
        }
    }

    // Salida del estado
    public override void SalirEstado()
    {
        // Si hay limpieza de efectos, se haría aquí
    }
}
