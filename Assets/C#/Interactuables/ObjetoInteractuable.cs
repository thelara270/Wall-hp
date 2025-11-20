using UnityEngine;

public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Datos del objeto")]
    public string nombre = "Objeto";     // Nombre del objeto
    public Sprite icono;                 // Ícono que se muestra en el inventario
    public Sprite iconoInput;            // Ícono que representa la acción (por ejemplo, tecla E)
    public Sprite iconoRecoger;          // Ícono que aparece cuando el jugador puede recoger el objeto

    private InventarioJugador inventario; // Referencia al inventario del jugador

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra en el área del objeto
        if (other.CompareTag("Player"))
        {
            // Obtiene el inventario del jugador
            inventario = other.GetComponent<InventarioJugador>();

            // Muestra el ícono de recoger en la UI
            if (iconoRecoger != null && UIInventario.Instance != null)
                UIInventario.Instance.MostrarIconoRecoger(iconoRecoger, true);

            // Indica al inventario qué objeto está cerca
            if (inventario != null)
                inventario.SetObjetoCerca(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del área
        if (other.CompareTag("Player") && inventario != null)
        {
            // Oculta el ícono de recoger
            if (UIInventario.Instance != null)
                UIInventario.Instance.MostrarIconoRecoger(null, false);

            // Limpia la referencia en el inventario
            inventario.SetObjetoCerca(null);
            inventario = null;
        }
    }

    // Acción básica al usar el objeto (puede sobreescribirse en clases hijas)
    public virtual void Usar()
    {
        Debug.Log("Usaste " + nombre);
    }

    // Comportamiento cuando el jugador recoge el objeto
    public void Recoger()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        // Desactiva físicas y colisión
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        // Oculta el ícono de recoger
        if (UIInventario.Instance != null)
            UIInventario.Instance.MostrarIconoRecoger(null, false);

        // Notificar al DialogoManager que se recogió un objeto (útil si un diálogo espera la acción)
        DialogoManager.instancia?.CumplirRequisito();

        // Opcional: desactivar el objeto en la escena si lo recoges en inventario
        // gameObject.SetActive(false);
    }

    // Comportamiento al soltar el objeto en el mundo
    public void Soltar(Vector3 posicion)
    {
        transform.position = posicion;

        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        // Activa nuevamente las físicas
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        // Reactiva el collider
        if (col != null)
            col.enabled = true;

        // Asegura que el objeto se vea en la escena
        gameObject.SetActive(true);

    }
}
