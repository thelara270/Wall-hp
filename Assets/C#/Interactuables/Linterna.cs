using UnityEngine;

public class Linterna : ObjetoInteractuable
{
    private Light luz;       //Referencia a la luz de la linterna
    private bool encendida;  //Estado actual de la linterna

    private void Awake()
    {
        //Busca la luz dentro del objeto y la desactiva al inicio
        luz = GetComponentInChildren<Light>();
        if (luz != null) luz.enabled = false;
    }

    //Sobrescribe el metodo Usar para encender o apagar la linterna
    public override void Usar()
    {
        if (luz == null) return;

        encendida = !encendida;
        luz.enabled = encendida;

        //UIInventario.Instance.MostrarMensaje("Linterna " + (encendida ? "encendida" : "apagada"));
    }
}
