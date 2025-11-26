using UnityEngine;

public class ZonaSiembra : MonoBehaviour
{
    [Header("Opciones de siembra")]
    [SerializeField] private string tagBolsa = "BolsaSemillas"; // Tag que identifica las bolsas de semillas

    private bool enRango = false;       // Indica si el jugador está dentro del área
    public bool ocupada = false;        // Indica si la zona ya tiene una planta
    private InventarioJugador jugador;  // Referencia al inventario del jugador

    private void Update()
    {
        // Si el jugador no está en rango o no existe, no hace nada
        if (!enRango || jugador == null) return;

        // Si la maceta ya tiene una planta, no se puede sembrar otra
        if (ocupada) return;

        // Verifica si el jugador sostiene una bolsa de semillas
        if (jugador.ObjetoEnMano != null && jugador.ObjetoEnMano.CompareTag(tagBolsa))
        {
            //UIInventario.Instance.MostrarMensaje("Presiona E para sembrar");

            // Si el jugador presiona E, se realiza la siembra
            if (Input.GetKeyDown(KeyCode.E))
                Sembrar(jugador.ObjetoEnMano);
        }
        else
        {
            // Limpia el mensaje si no hay objeto correcto en la mano
            //UIInventario.Instance.MostrarMensaje("");
        }
    }

    // Lógica para sembrar una planta
    private void Sembrar(ObjetoInteractuable bolsa)
    {
        if (bolsa == null) return;

        // Convierte el objeto a tipo BolsaArbol
        BolsaArbol bolsaSemilla = bolsa as BolsaArbol;
        if (bolsaSemilla == null) return;

        // Obtiene el prefab de la planta desde la bolsa
        GameObject prefab = bolsaSemilla.ObtenerPrefab();
        if (prefab == null) return;

        // Obtiene el animador del jugador para reproducir animación de siembra
        Animator anim = jugador.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetBool("Interactuando", true);

        // Calcula la posición donde aparecerá la planta
        Vector3 posicionSemilla = transform.position + Vector3.up * 1.3f;

        // Crea una instancia de la planta en la escena
        GameObject semillaObj = Instantiate(prefab, posicionSemilla, Quaternion.identity);

        // Asigna la zona de siembra a la planta
        if (semillaObj.TryGetComponent(out CrecimientoArbol crecimiento))
        {
            crecimiento.DarZonaSiembra(this);
        }

        AudioManager.instance.SonidoSembrar();


        // Elimina la bolsa del inventario del jugador
        jugador.UsarObjetoActivo();

        // Marca la maceta como ocupada
        ocupada = true;
        //UIInventario.Instance.MostrarMensaje("");

        // Termina la animación de siembra
        if (anim != null)
            anim.SetBool("Interactuando", false);
    }

    // Detecta cuando el jugador entra al área de siembra
    private void OnTriggerEnter(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null)
        {
            jugador = inv;
            enRango = true;
        }
    }

    // Detecta cuando el jugador sale del área
    private void OnTriggerExit(Collider other)
    {
        InventarioJugador inv = other.GetComponent<InventarioJugador>();
        if (inv != null && inv == jugador)
        {
            jugador = null;
            enRango = false;
            //UIInventario.Instance.MostrarMensaje("");
        }
    }

    // Libera la zona de siembra cuando la planta termina su ciclo
    public void LiberarMaceta()
    {
        ocupada = false;
    }
}
