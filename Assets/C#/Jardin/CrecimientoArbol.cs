using UnityEngine;
using System.Collections;
using System;

// Clase que controla el crecimiento de una planta o árbol en varias etapas.
public class CrecimientoArbol : MonoBehaviour
{
    // Configuración de crecimiento visible en el Inspector.
    [Header("Crecimiento")]
    public int etapaActual = 0;                 // Etapa actual del crecimiento (0 = semilla).
    public int etapaMaxima = 3;                 // Número máximo de etapas de crecimiento.
    public float aguaPorEtapa = 5f;             // Cantidad de agua necesaria para pasar de etapa.
    public float tiempoEsperaEntreFases = 3f;   // Tiempo de espera entre cada fase (cooldown).
    public Animator animator;                   // Controlador Animator que cambia las animaciones de crecimiento.
    public GameObject objetoEspera;             // Objeto visual que se muestra mientras la planta está en espera.

    // Variables privadas de control.
    private float agua = 0f;                    // Agua acumulada actualmente.
    private bool enCooldown = false;            // Indica si la planta está esperando entre fases.
    private bool regaderaEnRango = false;       // Indica si hay una regadera en el área de interacción.
    private Regadera regadera;                  // Referencia a la regadera detectada.
    private ZonaSiembra zonaSiembra;            // Referencia a la zona de siembra donde se encuentra esta planta.

    // Estado final del crecimiento.
    [Header("Estado final")]
    public bool plantaLista = false;            // Indica si la planta ya alcanzó la etapa máxima.



    // Evento que notifica cuando el árbol está completamente crecido.
    public event Action<CrecimientoArbol> OnArbolListo;

    private void Start()
    {
        // Si existe un GestorArboles, se registra este árbol al iniciarse.
        if (GestorArboles.Instance != null)
            GestorArboles.Instance.RegistrarArbol(this);
    }

    private void Awake()
    {
        // Busca la zona de siembra asociada (puede estar en el objeto padre).
        zonaSiembra = GetComponentInParent<ZonaSiembra>();

        // Si no se encuentra ninguna, muestra una advertencia en consola.
        if (zonaSiembra == null)
            Debug.LogWarning("No se encontró ZonaSiembra en los padres.");
    }

    // Permite establecer manualmente la zona de siembra desde otro script.
    public void DarZonaSiembra(ZonaSiembra zona)
    {
        zonaSiembra = zona;
    }

    private void Update()
    {
        // Si la planta ya alcanzó la etapa máxima, no hace nada.
        if (etapaActual >= etapaMaxima) return;

        // Si hay una regadera en rango y se presiona la tecla E.
        if (regaderaEnRango && Input.GetKeyDown(KeyCode.E))
        {
            // Si no está en cooldown y la regadera tiene agua disponible, riega la planta.
            if (!enCooldown && regadera != null && regadera.UsarAgua())
            {
                RecibirAgua(1f);
            }
            // Si la planta está en cooldown, muestra un mensaje al jugador.
            else if (enCooldown)
            {
                //UIInventario.Instance.MostrarMensaje("La planta está absorbiendo agua, espera un momento");
            }
        }
    }

    // Recibe una cantidad de agua y comprueba si puede avanzar de etapa.
    public void RecibirAgua(float cantidad)
    {
        // Si ya está en la etapa máxima o en cooldown, no procesa más agua.
        if (etapaActual >= etapaMaxima || enCooldown) return;

        // Suma la cantidad de agua recibida.
        agua += cantidad;
        AudioManager.instance.SonidoRegarPlanta();


        // Si alcanzó el umbral de agua necesario, avanza una etapa.
        if (agua >= aguaPorEtapa)
        {
            agua = 0;                      // Reinicia el contador de agua.
            Crecer();                      // Avanza la etapa de crecimiento.
            StartCoroutine(CooldownEntreFases());  // Activa el tiempo de espera antes de poder regar otra vez.
        }
    }

    // Incrementa la etapa de crecimiento y actualiza el animador.
    private void Crecer()
    {

        // Incrementa la etapa actual, sin pasar el máximo.
        etapaActual = Mathf.Min(etapaActual + 1, etapaMaxima);

        // Si hay un animador asignado, actualiza su parámetro "Etapa".
        if (animator != null)
            animator.SetInteger("Etapa", etapaActual);

        // Si se alcanzó la etapa máxima, marca la planta como lista y lanza el evento.
        if (etapaActual == etapaMaxima)
        {
            plantaLista = true;
            OnArbolListo?.Invoke(this);
        }
    }

    // Corrutina que maneja la espera entre fases y activa/desactiva efectos visuales.
    private IEnumerator CooldownEntreFases()
    {
        enCooldown = true; // Activa el estado de espera.

        // Si la planta no está completamente crecida, muestra el objeto de espera.
        if (etapaActual < etapaMaxima && objetoEspera != null)
            objetoEspera.SetActive(true);

        // Espera la cantidad de segundos configurada.
        yield return new WaitForSeconds(tiempoEsperaEntreFases);

        // Desactiva el objeto de espera cuando termina la espera.
        if (etapaActual < etapaMaxima && objetoEspera != null)
            objetoEspera.SetActive(false);

        enCooldown = false; // Termina el estado de espera.
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si el objeto que entra tiene una regadera.
        Regadera r = other.GetComponent<Regadera>();

        if (r != null)
        {
            regaderaEnRango = true;
            regadera = r;

            // Muestra mensaje al jugador indicando que puede regar.
            //UIInventario.Instance.MostrarMensaje("Presiona E para regar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detecta si la regadera salió del área de interacción.
        Regadera r = other.GetComponent<Regadera>();

        if (r != null && r == regadera)
        {
            regaderaEnRango = false;
            regadera = null;

            // Limpia el mensaje de la interfaz.
            //UIInventario.Instance.MostrarMensaje("");
        }
    }
}
