using UnityEngine;

public class EstadoExplosivoCargar : EstadoExplosivo
{
    private float tiempoRestante;

    public EstadoExplosivoCargar(ControladorExplosivo enemigo) : base(enemigo) { }

    public override void Enter()
    {
        enemigo.animator.SetInteger("Estado", 1); // Animación de carga
        tiempoRestante = enemigo.tiempoCarga;
    }

    public override void Update()
    {
        tiempoRestante -= Time.deltaTime;

        // Siempre mira al jugador
        Vector3 direccion = enemigo.jugador.position - enemigo.transform.position;
        direccion.y = 0;
        if (direccion != Vector3.zero)
        {
            enemigo.transform.rotation = Quaternion.Slerp(
                enemigo.transform.rotation,
                Quaternion.LookRotation(direccion),
                Time.deltaTime * 4f
            );
        }

        if (tiempoRestante <= 0)
        {
            enemigo.ChangeState(new EstadoExplosivoAtacar(enemigo));
        }
    }

    public override void Exit() { }
}
