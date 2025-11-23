using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoVida = 3f;
    public GameObject particulaImpacto;  // ← arrástrala desde el inspector

    private float tiempo;

    void OnEnable()
    {
        tiempo = 0;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);

        tiempo += Time.deltaTime;
        if (tiempo >= tiempoVida)
        {
            PoolProyectiles.Instance.RegresarBala(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (particulaImpacto != null)
                Instantiate(particulaImpacto, transform.position, Quaternion.identity);

            Debug.Log("🔴 Proyectil impactó al jugador");
            PoolProyectiles.Instance.RegresarBala(gameObject);  // 👈 Solo si es jugador
        }
    }
}
