using UnityEngine;

// Clase base abstracta para estados del boss
public abstract class EstadoBoss
{
    // Referencia al boss dueño del estado
    protected ControladorBossEva boss;

    // Constructor que recibe la referencia al boss
    public EstadoBoss(ControladorBossEva b)
    {
        boss = b;
    }

    // Método que se ejecuta al entrar al estado
    public abstract void EntrarEstado();

    // Método que se ejecuta cada frame mientras el estado está activo
    public abstract void ActualizarEstado();

    // Método que se ejecuta al salir del estado
    public abstract void SalirEstado();
}
