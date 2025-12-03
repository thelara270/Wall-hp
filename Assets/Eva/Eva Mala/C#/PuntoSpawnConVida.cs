using UnityEngine;

public class PuntoSpawnConVida : MonoBehaviour
{
    // Vida del punto de aparición
    public int vidaMaxima = 50;
    public int vidaActual = 50;

    // Bandera que indica si ya fue destruido
    public bool estaDestruido = false;

    // Control visual opcional
    public bool activarEfecto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    // Método llamado al recibir daño
    public void RecibirDaño(int cantidad)
    {
        if (estaDestruido)
            return;

        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            DestruirPunto();
        }
    }

    // Destruye el punto de aparición
    void DestruirPunto()
    {
        estaDestruido = true;

        // Aquí puedes poner partículas o animación
        gameObject.SetActive(false);
    }

    // Llamado por el estado del boss cuando inicia la fase
    public void ActivarPunto()
    {
        gameObject.SetActive(true);
    }

    // Detecta daño de balas
    private void OnCollisionEnter(Collision collision)
    {
        DañoEnemigo daño = collision.gameObject.GetComponent<DañoEnemigo>();
        if (daño != null)
            RecibirDaño(daño.daño);
    }

    private void OnTriggerEnter(Collider other)
    {
        DañoEnemigo daño = other.GetComponent<DañoEnemigo>();
        if (daño != null)
            RecibirDaño(daño.daño);
    }
}
