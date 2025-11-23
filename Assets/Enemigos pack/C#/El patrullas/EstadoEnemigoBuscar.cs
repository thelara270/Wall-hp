using UnityEngine;

public class EstadoEnemigoBuscar : EstadoEnemigo
{
    private float tiempoRestante;

    public EstadoEnemigoBuscar(ControladorEnemigo enemigo) : base(enemigo) { }

    public override void Enter()
    {
        enemigo.agent.isStopped = true;
        enemigo.animator.SetInteger("Estado", 0); // idle
        tiempoRestante = enemigo.tiempoBusqueda;
    }

    public override void Update()
    {
        tiempoRestante -= Time.deltaTime;

        if (enemigo.DetectarJugador())
            enemigo.ChangeState(enemigo.GetChaseState());
        else if (tiempoRestante <= 0)
            enemigo.ChangeState(enemigo.GetReturnState());
    }

    public override void Exit() { }
}
