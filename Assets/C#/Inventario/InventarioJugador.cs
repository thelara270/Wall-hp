using UnityEngine;
using UnityEngine.UI;

public class InventarioJugador : MonoBehaviour
{
    [Header("Slots del inventario")]
    public ObjetoInteractuable[] slots = new ObjetoInteractuable[3]; // Máximo 3 espacios en el inventario
    public Image[] iconosSlots; // Referencias a los íconos de los slots en la UI

    public Transform puntoMano; // Donde se muestra el objeto en la mano del jugador

    private ObjetoInteractuable objetoCerca;   // Objeto que está cerca y puede recogerse
    public ObjetoInteractuable objetoEnMano;   // Objeto actualmente equipado en la mano
    private int slotActivo = -1;               // Índice del slot activo

    private Animator animator; // Referencia al Animator del jugador

    public ObjetoInteractuable ObjetoEnMano => objetoEnMano; // Propiedad para obtener el objeto en mano
    public int SlotActivo => slotActivo;                     // Propiedad para obtener el slot activo

    private void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el componente Animator del jugador
    }

    private void Update()
    {
        // Cambio de slot con teclas numéricas
        if (Input.GetKeyDown(KeyCode.Alpha1)) CambiarSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) CambiarSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) CambiarSlot(2);

        // Recoger o soltar objetos con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (objetoCerca != null) // Recoger objeto cercano
            {
                AgregarObjeto(objetoCerca);
                AudioManager.instance?.SonidoTomarObjeto();
                //UIInventario.Instance.MostrarMensaje("");
                objetoCerca = null;
            }
            else if (objetoEnMano != null) // Soltar objeto en mano
            {
                SoltarObjeto(slotActivo);
            }
        }

        // Usar el objeto en mano con E
        if (Input.GetKeyDown(KeyCode.E) && objetoEnMano != null)
        {
            objetoEnMano.Usar();
        }

        // Mostrar icono de input si hay objeto en mano
        MostrarIconoInput();

        // Mensajes de interacción en pantalla
        if (objetoEnMano != null)
        {
            //UIInventario.Instance.MostrarMensaje("Presiona E para interactuar con " + objetoEnMano.nombre);
        }
        else if (objetoCerca == null)
        {
            //UIInventario.Instance.MostrarMensaje("");
        }
    }

    // Agrega un objeto al inventario
    public void AgregarObjeto(ObjetoInteractuable obj)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) // Busca un slot vacío
            {
                slots[i] = obj;

                // Desactiva el objeto en la escena
                obj.Recoger();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(false);

                // Si no hay slot activo, este se activa automáticamente
                if (slotActivo == -1)
                    CambiarSlot(i);
                // Si el slot activo coincide con el nuevo, se equipa de inmediato
                else if (slotActivo == i)
                    CambiarSlot(i);

                // Actualiza la UI
                UIInventario.Instance.ActualizarSlots(slots, slotActivo);
                ActualizarIconosSlots();

                //UIInventario.Instance.MostrarMensaje(obj.nombre + " agregado al inventario");

                // Actualiza animación
                if (animator != null)
                    animator.SetBool("Interactuando", true);

                return;
            }
        }

        // Si no hay espacio disponible
        //UIInventario.Instance.MostrarMensaje("Inventario lleno!");
    }

    // Cambia el slot activo del inventario
    public void CambiarSlot(int nuevoSlot)
    {
        if (nuevoSlot < 0 || nuevoSlot >= slots.Length) return;

        // Oculta el objeto anterior
        if (objetoEnMano != null)
        {
            objetoEnMano.gameObject.SetActive(false);
            objetoEnMano.transform.SetParent(null);
            objetoEnMano = null;
        }

        slotActivo = nuevoSlot;

        // Muestra el nuevo objeto en mano
        if (slots[slotActivo] != null)
        {
            objetoEnMano = slots[slotActivo];
            objetoEnMano.gameObject.SetActive(true);
            objetoEnMano.transform.SetParent(puntoMano);
            objetoEnMano.transform.localPosition = Vector3.zero;
            objetoEnMano.transform.localRotation = Quaternion.identity;
        }

        // Actualiza la UI
        UIInventario.Instance.ActualizarSlots(slots, slotActivo);
        ActualizarIconosSlots();

        // Actualiza animación
        if (animator != null)
            animator.SetBool("Interactuando", objetoEnMano != null);
    }

    // Muestra el icono de input si el objeto está en mano
    private void MostrarIconoInput()
    {
        Image iconoUI = GameObject.Find("IconoInteraccion")?.GetComponent<Image>();
        if (iconoUI == null) return;

        if (objetoEnMano != null && objetoEnMano.iconoInput != null)
        {
            iconoUI.sprite = objetoEnMano.iconoInput;
            iconoUI.enabled = true;
        }
        else
        {
            iconoUI.enabled = false;
        }
    }

    // Suelta un objeto del inventario
    public void SoltarObjeto(int slotIndex)
    {
        if (slots[slotIndex] != null)
        {
            ObjetoInteractuable objeto = slots[slotIndex];
            objeto.transform.SetParent(null);

            // Posición frente al jugador
            Vector3 posicionSoltar = transform.position + transform.forward * 1f;
            objeto.Soltar(posicionSoltar);
            objeto.gameObject.SetActive(true);
            AudioManager.instance?.SonidoTomarObjeto();

            //UIInventario.Instance.MostrarMensaje("Soltaste " + objeto.nombre);

            // Lo elimina del inventario
            slots[slotIndex] = null;
            if (slotActivo == slotIndex) objetoEnMano = null;

            // Actualiza la UI
            UIInventario.Instance.ActualizarSlots(slots, slotActivo);
            ActualizarIconosSlots();

            // Actualiza animación
            if (animator != null)
                animator.SetBool("Interactuando", objetoEnMano != null);
        }
    }

    // Usa y elimina un objeto activo
    public void UsarObjetoActivo()
    {
        if (slots[slotActivo] != null)
        {
            ObjetoInteractuable usado = slots[slotActivo];
            slots[slotActivo] = null;
            objetoEnMano = null;

            if (usado != null)
            {
                usado.transform.SetParent(null);
                Destroy(usado.gameObject);
            }

            UIInventario.Instance.ActualizarSlots(slots, slotActivo);
            ActualizarIconosSlots();

            // Actualiza animación
            if (animator != null)
                animator.SetBool("Interactuando", false);
        }
    }

    // Define el objeto cercano
    public void SetObjetoCerca(ObjetoInteractuable obj) => objetoCerca = obj;

    // Actualiza los íconos de los slots en la UI
    private void ActualizarIconosSlots()
    {
        for (int i = 0; i < iconosSlots.Length; i++)
        {
            if (i < slots.Length && slots[i] != null && slots[i].icono != null)
            {
                iconosSlots[i].sprite = slots[i].icono;
                iconosSlots[i].enabled = true;
            }
            else
            {
                iconosSlots[i].sprite = null;
                iconosSlots[i].enabled = false;
            }

            // Cambia transparencia si el slot no está activo
            iconosSlots[i].color = (i == slotActivo) ? Color.white : new Color(1, 1, 1, 0.5f);
        }
    }
}
