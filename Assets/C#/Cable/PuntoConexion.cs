using UnityEngine;
using UnityEngine.EventSystems;

public class PuntoConexion : MonoBehaviour, IDropHandler
{
    public string colorEsperado;      //El color que debe coincidir

    public MinijuegoCables minijuego; //Referencia al minijuego de cables

    public void OnDrop(PointerEventData eventData)
    {
        //Obtiene el cable que se esta soltando
        Cable cable = eventData.pointerDrag.GetComponent<Cable>();

        //Si lo que soltamos realmente es un cable
        if (cable != null)
        {
            //Verifica si el color coincide con este punto
            if (cable.colorCable == colorEsperado)
            {
                Debug.Log("Cable correcto: " + cable.colorCable);

                //Avisa al minijuego que se completo la conexion
                minijuego.CompletarConexion();

                //Desactiva el cable colocado
               cable.gameObject.SetActive(false);
            }
            else
            {
                //Mensaje si el cable no coincide
                Debug.Log("Cable incorrecto.");
            }
        }
    }
}
