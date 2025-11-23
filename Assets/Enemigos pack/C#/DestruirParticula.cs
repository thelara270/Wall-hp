using UnityEngine;

public class DestruirParticula : MonoBehaviour
{
    private ParticleSystem sistemaParticulas;

    void Start()
    {
        // 1. Intentar obtener el componente ParticleSystem en este mismo GameObject.
        sistemaParticulas = GetComponent<ParticleSystem>();

        // 2. Verificar si el sistema de partículas existe.
        if (sistemaParticulas != null)
        {
            // 3. Obtener la duración configurada en el módulo Main del sistema.
            float duracionTotal = sistemaParticulas.main.duration;

            // 4. Destruir el GameObject (y por lo tanto el sistema de partículas)
            //    después de que la duración del sistema haya terminado.
            Destroy(gameObject, duracionTotal);
        }
    }
}