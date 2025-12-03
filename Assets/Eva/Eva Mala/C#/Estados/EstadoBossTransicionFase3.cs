using UnityEngine;

public class EstadoBossTransicionFase3 : EstadoBoss
{
    float tiempo = 2f;

    public EstadoBossTransicionFase3(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idFase3);
        Debug.Log("¡¡ EVA entra en la FASE 3 !!");
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
        {
            // Aquí irá el inicio de Fase 3 real
        }
    }

    public override void SalirEstado() { }
}
