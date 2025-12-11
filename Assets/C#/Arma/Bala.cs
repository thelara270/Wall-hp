using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 40f;
    public float vida = 2f;

    private float tiempoDestruir;
    private Arma armaOrigen;

    public GameObject particulasImpacto;

    Vector3 direccion;

    [Header("Colisiones Permitidas")]
    public LayerMask layersPermitidas;

    public void Disparar(Vector3 dir, Arma arma)
    {
        direccion = dir;
        armaOrigen = arma;
        tiempoDestruir = Time.time + vida;

        transform.rotation = Quaternion.LookRotation(dir);

        // IGNORAR EL COLLIDER DEL JUGADOR
        Collider colBala = GetComponent<Collider>();
        Collider colJugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        if (colBala != null && colJugador != null)
            Physics.IgnoreCollision(colBala, colJugador, true);
    }


    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;

        if (Time.time >= tiempoDestruir)
            armaOrigen.RetornarBala(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        // Evitar colisión con el jugador
        if (col.collider.CompareTag("Player"))
            return;

        // Revisar si el objeto colisionado está en una layer permitida
        if ((layersPermitidas.value & (1 << col.gameObject.layer)) == 0)
            return;

        if (particulasImpacto != null)
        {
            GameObject p = Instantiate(particulasImpacto, transform.position, Quaternion.identity);
            Destroy(p, 0.5f);
        }

        armaOrigen.RetornarBala(gameObject);
    }
}
