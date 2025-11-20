using UnityEngine;

//Clase que representa un fusible como objeto interactuable
public class Fusible : ObjetoInteractuable
{
    private void Awake()
    {
        //Al iniciar le damos el nombre de Fusible
        nombre = "Fusible";
    }

    //Sobrescribimos el metodo Usar para este objeto
    public override void Usar()
    {
        //Muestra un mensaje en la UI
        //UIInventario.Instance.MostrarMensaje("Necesitas colocarlo en la caja de fusibles");
    }
}
