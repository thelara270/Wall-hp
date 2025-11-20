using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DesvanecerAlIniciar : MonoBehaviour
{
    public float duracionFade = 1.5f;     // Duración del fade
    public float tiempoEspera = 1f;       // Tiempo antes de empezar a desvanecer
    private Image imagenPantalla;

    void Start()
    {
        imagenPantalla = GetComponent<Image>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Mantener la pantalla negra para que el jugador se instancie
        yield return new WaitForSeconds(tiempoEspera);

        Color colorActual = imagenPantalla.color;

        for (float tiempo = 0; tiempo < duracionFade; tiempo += Time.deltaTime)
        {
            colorActual.a = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            imagenPantalla.color = colorActual;
            yield return null;
        }

        colorActual.a = 0f;
        imagenPantalla.color = colorActual;
        gameObject.SetActive(false);
    }
}
