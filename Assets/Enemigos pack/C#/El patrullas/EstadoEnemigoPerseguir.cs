using UnityEngine;

public class EstadoEnemigoPerseguir : EstadoEnemigo
{
    public EstadoEnemigoPerseguir(ControladorEnemigo enemigo) : base(enemigo) { }

    public override void Enter()
    {
        enemigo.agent.isStopped = false;
        enemigo.animator.SetInteger("Estado", 2); // correr
        enemigo.tiempoPersiguiendo = 0f;
    }

    public override void Update()
    {
        Vector3 direccion = enemigo.jugador.position - enemigo.transform.position;
        float distancia = direccion.magnitude;

        // 🔹 Si aún está lejos, se acerca
        if (distancia > enemigo.distanciaMinima)
        {
            Vector3 puntoObjetivo = enemigo.jugador.position - direccion.normalized * enemigo.distanciaMinima;
            enemigo.agent.SetDestination(puntoObjetivo);
        }
        else
        {
            // 🔹 Si ya está a distancia justa, se queda quieto pero mira hacia el jugador
            enemigo.agent.SetDestination(enemigo.transform.position);
            enemigo.transform.LookAt(new Vector3(enemigo.jugador.position.x, enemigo.transform.position.y, enemigo.jugador.position.z));
        }

        enemigo.tiempoPersiguiendo += Time.deltaTime;

        if (enemigo.enCooldown)
        {
            enemigo.ChangeState(enemigo.GetReturnState());
            return;
        }

        if (enemigo.tiempoPersiguiendo >= enemigo.tiempoParaInstanciar)
        {
            enemigo.buscarAlLlegar = true;
            enemigo.InstanciarExplosivos();
            enemigo.ChangeState(enemigo.GetReturnState());
        }
        else if (!enemigo.DetectarJugador())
        {
            enemigo.tiempoPersiguiendo = 0;
            enemigo.buscarAlLlegar = false;
            enemigo.ChangeState(enemigo.GetSearchState());
        }
    }

    public override void Exit() { }
}
