using UnityEngine;

public class EstadoEnemigoPatrullaje : EstadoEnemigo
{
    public EstadoEnemigoPatrullaje(ControladorEnemigo enemigo) : base(enemigo) { }

    public override void Enter()
    {
        enemigo.agent.isStopped = false;
        enemigo.animator.SetInteger("Estado", 1); // caminando
    }

    public override void Update()
    {
        if (enemigo.puntosPatrulla.Length == 0) return;

        enemigo.agent.SetDestination(enemigo.puntosPatrulla[enemigo.indicePatrulla].position);

        if (!enemigo.agent.pathPending &&
            enemigo.agent.remainingDistance <= enemigo.agent.stoppingDistance)
        {
            enemigo.indicePatrulla = (enemigo.indicePatrulla + 1) % enemigo.puntosPatrulla.Length;
        }

        if (enemigo.DetectarJugador())
            enemigo.ChangeState(enemigo.GetChaseState());
    }

    public override void Exit() { }
}
