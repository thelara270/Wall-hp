using UnityEngine;

public class EstadoEnemigoVolver : EstadoEnemigo
{
    public EstadoEnemigoVolver(ControladorEnemigo enemy) : base(enemy) { }

    public override void Enter()
    {
        enemigo.agent.isStopped = false;
        enemigo.animator.SetInteger("Estado", 1); // caminar
        enemigo.agent.SetDestination(enemigo.posicionInicio);
    }

    public override void Update()
    {
        if (!enemigo.agent.pathPending &&
            enemigo.agent.remainingDistance <= enemigo.agent.stoppingDistance)
        {
            if (enemigo.buscarAlLlegar)
            {
                enemigo.buscarAlLlegar = false;
                enemigo.ChangeState(enemigo.GetSearchState());
            }
            else
            {
                enemigo.ChangeState(enemigo.GetPatrolState());
            }
            return;
        }
    }

    public override void Exit() { }
}
