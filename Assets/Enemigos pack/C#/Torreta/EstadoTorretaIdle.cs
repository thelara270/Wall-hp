using UnityEngine;

public class EstadoTorretaIdle : EstadoTorreta
{
    public EstadoTorretaIdle(ControladorTorreta torreta) : base(torreta) { }

    public override void Enter()
    {
        torreta.animator.SetInteger("Estado", 0);
        torreta.luzDeteccion.enabled = false;
    }

    public override void Update()
    {
        if (Vector3.Distance(torreta.transform.position, torreta.jugador.position) <= torreta.rangoDeteccion)
        {
            torreta.ChangeState(torreta.GetDetectState());
        }
    }

    public override void Exit() { }
}
