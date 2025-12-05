using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DesvanecerPantalla : MonoBehaviour
{
    public float duracionFade = 1.5f;
    private Image imagenPantalla;

    private void Awake()
    {
        imagenPantalla = GetComponent<Image>();
    }

    // -------------------------
    // FADE IN (Pantalla negra → transparente)
    // -------------------------
    public void IniciarFadeIn(float tiempoEspera = 0f)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn(tiempoEspera));
    }

    IEnumerator FadeIn(float tiempoEspera)
    {
        yield return new WaitForSeconds(tiempoEspera);

        Color colorActual = imagenPantalla.color;

        for (float t = 0; t < duracionFade; t += Time.deltaTime)
        {
            colorActual.a = Mathf.Lerp(1f, 0f, t / duracionFade);
            imagenPantalla.color = colorActual;
            yield return null;
        }

        colorActual.a = 0f;
        imagenPantalla.color = colorActual;
        gameObject.SetActive(false);
    }

    // -------------------------
    // FADE OUT (Transparente → pantalla negra)
    // -------------------------
    public void IniciarFadeOut()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color colorActual = imagenPantalla.color;

        for (float t = 0; t < duracionFade; t += Time.deltaTime)
        {
            colorActual.a = Mathf.Lerp(0f, 1f, t / duracionFade);
            imagenPantalla.color = colorActual;
            yield return null;
        }

        colorActual.a = 1f;
        imagenPantalla.color = colorActual;

        // No se desactiva porque quieres que quede oscuro
    }
}
