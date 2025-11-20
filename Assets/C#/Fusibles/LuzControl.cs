using UnityEngine;
using System.Collections;

public class LuzParpadeo : MonoBehaviour
{
    [Header("Configuración de Luz")]
    public Light luz;                          // Luz a controlar
    public float intensidadEncendida = 2f;     // Intensidad máxima
    public float intensidadApagada = 0f;       // Intensidad mínima
    public float velocidadTransicion = 2f;     // Qué tan rápido cambia la intensidad

    [Header("Parpadeo Automático")]
    public float tiempoEncendida = 1f;         // Cuánto tiempo permanece encendida
    public float tiempoApagada = 0.5f;         // Cuánto tiempo permanece apagada
    public bool parpadear = true;              // Activar o desactivar parpadeo

    [Header("Color Fijo al Cambiar")]
    public float intensidadFija = 1.5f;        // Intensidad al cambiar color

    public Color nuevoColor;

    private Coroutine rutinaActual;

    void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();

        if (parpadear)
            StartCoroutine(ParpadeoAutomatico());
    }

    // 🔆 Encender la luz (transición suave)
    public void EncenderLuz()
    {
        if (rutinaActual != null) StopCoroutine(rutinaActual);
        rutinaActual = StartCoroutine(CambiarIntensidad(intensidadEncendida));
    }

    // 💡 Apagar la luz (transición suave)
    public void ApagarLuz()
    {
        if (rutinaActual != null) StopCoroutine(rutinaActual);
        rutinaActual = StartCoroutine(CambiarIntensidad(intensidadApagada));
    }

    // 🎨 Cambiar color y fijar intensidad
    public void CambiarColor()
    {
        // Detiene el parpadeo
        parpadear = false;
        StopAllCoroutines();

        // Cambia color e intensidad fija
        luz.color = nuevoColor;
        luz.intensity = intensidadFija;
    }

    // ⚙️ Corrutina para interpolar la intensidad
    IEnumerator CambiarIntensidad(float nuevaIntensidad)
    {
        float intensidadInicial = luz.intensity;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidadTransicion;
            luz.intensity = Mathf.Lerp(intensidadInicial, nuevaIntensidad, t);
            yield return null;
        }

        luz.intensity = nuevaIntensidad;
    }

    // 🔁 Corrutina principal de parpadeo
    IEnumerator ParpadeoAutomatico()
    {
        while (true)
        {
            EncenderLuz();
            yield return new WaitForSeconds(tiempoEncendida);

            ApagarLuz();
            yield return new WaitForSeconds(tiempoApagada);
        }
    }
}
