using UnityEngine;

public class EstadoBossTransicionFase3 : EstadoBoss
{
    float tiempo = 2f;

    public EstadoBossTransicionFase3(ControladorBossEva b) : base(b) { }

    public override void EntrarEstado()
    {
        boss.SetAnimadorEstado(boss.idFase3);
        Debug.Log("¡¡ EVA entra en la FASE 3 !!");

        // -----------------------------------
        // 🔥 ACTIVAR TORRETAS Y PONER RANGO 0
        // -----------------------------------
        if (boss.torretasFase3 != null)
        {
            foreach (var torreta in boss.torretasFase3)
            {
                if (torreta == null) continue;

                // Activamos el GameObject
                torreta.gameObject.SetActive(true);

                // Guardamos su rango real (se usa luego en EstadoCansadaTorretas)
                torreta.rangoDeteccion = 0f;

                // FORZAR A ESTAR INACTIVA mientras dura la transición
                torreta.ChangeState(torreta.GetIdleState());
            }
        }
    }

    public override void ActualizarEstado()
    {
        tiempo -= Time.deltaTime;

        if (tiempo <= 0)
        {
            // Pasar al estado de láseres
            boss.CambiarEstado(boss.GetEstadoLaserCombinado());
        }
    }

    public override void SalirEstado()
    {
        // Nada que hacer aquí por ahora
    }
}
