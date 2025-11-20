using UnityEngine;

public class ZonaAgua : MonoBehaviour
{
    private bool enRango = false;     // Indica si la regadera está dentro del área
    private Regadera regadera;        // Referencia al componente de la regadera

    // Se ejecuta cuando un objeto entra en la zona
    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto tiene la etiqueta "Regadera"
        if (other.CompareTag("Regadera"))
        {
            enRango = true;
            regadera = other.GetComponent<Regadera>();

            // Muestra mensaje para recargar agua
            //UIInventario.Instance.MostrarMensaje("Presiona E para llenar la regadera");
        }
    }

    // Se ejecuta cuando el objeto sale del área
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Regadera"))
        {
            enRango = false;
            regadera = null;

            // Limpia el mensaje
            //UIInventario.Instance.MostrarMensaje("");
        }
    }

    // Comprueba si se presiona E para llenar la regadera
    private void Update()
    {
        if (enRango && Input.GetKeyDown(KeyCode.E) && regadera != null)
        {
            bool recargando = regadera.RecargarAgua();
        }
    }
}
