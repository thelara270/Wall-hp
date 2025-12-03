using UnityEngine;

// Estado de generación de enemigos 911 y bombas de prueba
public class EstadoBossSpawn911 : EstadoBoss
{
    // Temporizador interno para spawn de bombas
    private float tiempo;

    // Intervalo de tiempo entre cada oleada de bombas
    private float intervaloBombas;

    // Prefab de la bomba
    private GameObject prefabBomba;

    // Constructor del estado
    public EstadoBossSpawn911(ControladorBossEva b) : base(b)
    {
        // Obtiene el intervalo configurado en el controlador
        intervaloBombas = b.intervaloBombasPrueba;

        // Obtiene el prefab desde el controlador
        prefabBomba = b.prefabBombaPrueba;
    }

    // Entrada al estado
    public override void EntrarEstado()
    {
        // Reinicia el temporizador
        tiempo = 0f;

        // Pone al boss en animación idle mientras espera
        boss.SetAnimadorEstado(boss.idIdleFase1);

        // Activa los puntos de spawn (por si estaban apagados)
        foreach (var p in boss.puntosSpawn)
        {
            if (p != null)
                p.ActivarPunto();
        }
    }

    // Actualización del estado
    public override void ActualizarEstado()
    {
        // Aumenta el temporizador
        tiempo += Time.deltaTime;

        // Si supera el intervalo, lanza bombas
        if (tiempo >= intervaloBombas)
        {
            tiempo = 0f;
            boss.SetAnimadorEstado(boss.idSpawn911);
            LanzarBombasDesdePuntos();
        }
        else
        {
            boss.SetAnimadorEstado(boss.idIdleFase1);
        }

        // Si todos los puntos de spawn están destruidos, cambia de estado
        if (boss.TodosLosPuntosDestruidos())
        {
            boss.CambiarEstado(boss.ObtenerTransicionFase2());
            tiempo = 0;
        }
    }


    // Lanza una bomba en cada punto de spawn que no esté destruido
    void LanzarBombasDesdePuntos()
    {
        foreach (var p in boss.puntosSpawn)
        {
            // Si el punto existe y no está destruido
            if (p != null && !p.estaDestruido)
            {
                // Instancia una bomba en la posición del punto
                Object.Instantiate(prefabBomba, p.transform.position, p.transform.rotation);
            }
        }
    }

    // Salida del estado
    public override void SalirEstado()
    {
        // No necesita lógica al salir
    }
}
