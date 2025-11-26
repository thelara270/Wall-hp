using UnityEngine;

public class ZonaFusibles : MonoBehaviour
{
    [Header("Fusibles")]
    public int totalFusibles = 3;   //Cantidad total de fusibles necesarios
    private int colocados = 0;      //Cantidad de fusibles ya colocados
    private bool enRango;           //Indica si el jugador está en la zona

    [SerializeField] MisionSala1 mision; //Referencia a la mision

    private void Update()
    {
        //Si el jugador esta en rango y presiona la tecla E
        if (enRango && Input.GetKeyDown(KeyCode.E))
        {
            //Obtenemos el inventario del jugador
            InventarioJugador inventario = FindObjectOfType<InventarioJugador>();

            //Verifica si el jugador tiene un objeto en la mano y que sea un fusible
            if (inventario != null && inventario.ObjetoEnMano != null && inventario.ObjetoEnMano.nombre == "Fusible")
            {
                //Si aun faltan fusibles por colocar
                if (colocados < totalFusibles)
                {
                    colocados++; //Sumamos un fusible
                    Debug.Log("Fusible colocado: " + colocados + "/" + totalFusibles);

                    //lo quitamos del inventario del jugador
                    inventario.UsarObjetoActivo();
                    AudioManager.instance?.SonidoPanelElectrico();


                    // Notificar al DialogoManager que se colocó un fusible (esto cumple requisitos vinculados)
                    DialogoManager.instancia?.CumplirRequisito();
                }

                //Si ya se colocaron todos los fusibles
                if (colocados >= totalFusibles)
                {
                    Debug.Log("Todos los fusibles colocados. Generador encendido");

                    //Marca el progreso en la mision
                    if (mision != null)
                    {
                        mision.fusiblesColocados = true;
                        mision.generadorReparado = true;
                    }

                    // Notificar al DialogoManager que la tarea completa fue realizada
                    DialogoManager.instancia?.CumplirRequisito();
                }
            }
            else
            {
                //Mensaje si intenta interactuar sin tener fusibles en la mano
                Debug.Log("Necesitas tener un fusible en la mano");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enRango = true; //Marca que esta en rango
            Debug.Log("Presiona E para colocar un fusible");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enRango = false; //Marca que salio del rango
        }
    }
}
