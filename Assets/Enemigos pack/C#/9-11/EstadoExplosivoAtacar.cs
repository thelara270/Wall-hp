using UnityEngine;

public class EstadoExplosivoAtacar : EstadoExplosivo
{
    public EstadoExplosivoAtacar(ControladorExplosivo enemigo) : base(enemigo) { }

    public override void Enter()
    {
        enemigo.animator.SetInteger("Estado", 2); // Animación correr
    }

    public override void Update()
    {
        // Dirección hacia el jugador
        Vector3 dir = (enemigo.jugador.position - enemigo.transform.position).normalized;
        dir.y = 0;

        // =========================
        // ROTACIÓN HACIA EL JUGADOR
        // =========================
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            enemigo.transform.rotation = Quaternion.Slerp(
                enemigo.transform.rotation,
                rot,
                enemigo.velocidadAtaque * Time.deltaTime
            );
        }

        // =========================
        // DETECCIÓN POR RADIO (OverlapSphere)
        // =========================
        Collider[] hits = Physics.OverlapSphere(enemigo.transform.position, enemigo.radioDeteccionPared, ~0, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider c = hits[i];
            if (c == null) continue;
            if (c.transform == enemigo.transform || c.CompareTag("Player")) continue;

            if (c.CompareTag(enemigo.tagParedes))
            {
                Debug.Log($"💥 Explosivo detectó pared '{c.name}' en radio {enemigo.radioDeteccionPared} -> explota");
                enemigo.Explode();
                return;
            }
        }

        // =========================
        // MOVIMIENTO HACIA EL JUGADOR
        // =========================
        enemigo.transform.position += dir * enemigo.velocidadAtaque * Time.deltaTime;

        // Si está suficientemente cerca del jugador, explota
        if (Vector3.Distance(enemigo.transform.position, enemigo.jugador.position) < enemigo.distanciaExplosion)
        {
            enemigo.Explode(true); // Solo daño directo
        }
    }

    public override void Exit() { }
}
