using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoAim : MonoBehaviour
{
    [Header("Auto Aim")]
    public float rango = 20f;
    public LayerMask enemyLayer;
    public GameObject miraCanvas;

    private Transform enemigoActual;
    private bool apuntando = false;

    void Update()
    {
        // Activar apuntado
        if (Input.GetMouseButtonDown(1))
        {
            apuntando = true;
            enemigoActual = BuscarEnemigoMasCercano();

            if (enemigoActual != null)
                miraCanvas.SetActive(true);
        }

        // Desactivar apuntado
        if (Input.GetMouseButtonUp(1))
        {
            apuntando = false;
            enemigoActual = null;
            miraCanvas.SetActive(false);
        }

        // Si estamos apuntando, mirar al enemigo
        if (apuntando && enemigoActual != null)
        {
            ApuntarAlEnemigo();
        }
    }

    Transform BuscarEnemigoMasCercano()
    {
        Collider[] encontrados = Physics.OverlapSphere(transform.position, rango, enemyLayer);

        if (encontrados.Length == 0)
            return null;

        return encontrados
            .OrderBy(col => Vector3.Distance(transform.position, col.transform.position))
            .First()
            .transform;
    }

    void ApuntarAlEnemigo()
    {
        Vector3 direccion = enemigoActual.position - transform.position;
        direccion.y = 0;

        Quaternion rotObjetivo = Quaternion.LookRotation(direccion);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            rotObjetivo,
            10f * Time.deltaTime
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rango);
    }
}