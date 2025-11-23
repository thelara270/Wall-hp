using UnityEngine;

public class EstadoTorretaDisparar : EstadoTorreta
{
    public float tiempoEntreDisparos = 0.5f;
    private float tiempoDisparo;

    public EstadoTorretaDisparar(ControladorTorreta torreta) : base(torreta) { }

    public override void Enter()
    {
        torreta.animator.SetInteger("Estado", 2);
        tiempoDisparo = 0f;
    }

    public override void Update()
    {
        torreta.transform.LookAt(torreta.jugador);
        tiempoDisparo += Time.deltaTime;

        if (tiempoDisparo >= tiempoEntreDisparos)
        {
            Disparar();
            tiempoDisparo = 0f;
        }

        if (Vector3.Distance(torreta.transform.position, torreta.jugador.position) > torreta.rangoDeteccion)
        {
            torreta.ChangeState(torreta.GetIdleState());
        }
    }

    void Disparar()
    {
        GameObject bala = PoolProyectiles.Instance.ObtenerBala();
        if (bala == null) return;

        bala.transform.position = torreta.puntoDisparo.position;
        bala.transform.rotation = torreta.puntoDisparo.rotation;
        bala.SetActive(true); // 🔹 ACTIVAR AL FINAL
    }

    public override void Exit() { }
}
