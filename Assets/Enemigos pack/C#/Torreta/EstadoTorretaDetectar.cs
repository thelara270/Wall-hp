using UnityEngine;

public class EstadoTorretaDetectar : EstadoTorreta
{
    private float tiempo = 0f;

    public EstadoTorretaDetectar(ControladorTorreta torreta) : base(torreta) { }

    public override void Enter()
    {
        torreta.animator.SetInteger("Estado", 1);
        torreta.luzDeteccion.enabled = true;
        tiempo = 0f;
    }

    public override void Update()
    {
        tiempo += Time.deltaTime;
        torreta.transform.LookAt(torreta.jugador);

        if (tiempo >= torreta.tiempoAntesDeDisparar)
        {
            torreta.ChangeState(torreta.GetShootState());
        }
    }

    public override void Exit() { }
}
