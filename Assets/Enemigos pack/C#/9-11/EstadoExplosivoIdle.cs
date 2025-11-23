using UnityEngine;

public class EstadoExplosivoIdle : EstadoExplosivo
{
    public EstadoExplosivoIdle(ControladorExplosivo enemigo) : base(enemigo) { }

    public override void Enter()
    {
        enemigo.animator.SetInteger("Estado", 0); // Animación Idle
    }

    public override void Update()
    {
        float distancia = Vector3.Distance(enemigo.transform.position, enemigo.jugador.position);

        if (distancia < enemigo.radioDeteccion)
        {
            enemigo.ChangeState(new EstadoExplosivoCargar(enemigo));
        }
    }

    public override void Exit() { }
}
