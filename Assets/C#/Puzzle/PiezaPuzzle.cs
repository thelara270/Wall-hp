using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PiezaPuzzle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Configuración del encaje")]
    public RectTransform zonaObjetivo;       // Zona donde debe encajar la pieza
    public float rangoSnap = 20f;            // Tolerancia de encaje en píxeles
    [Range(0, 45)] public float toleranciaRotacion = 10f; // Tolerancia angular

    [Header("Configuración de posición aleatoria")]
    public bool usarPosicionAleatoria = true;
    public Vector2 margenBordes = new Vector2(50f, 50f);
    public float distanciaMinimaAlObjetivo = 150f;
    public bool evitarSolapamiento = true;
    public float margenSeparacion = 15f;

    [Header("Configuración de rotación")]
    public bool usarRotacionAleatoria = true;
    public bool permitirRotacionManual = true;
    public float incrementoRotacion = 90f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 posicionInicial;
    private Quaternion rotacionInicial;

    private static List<PiezaPuzzle> todasLasPiezas = new List<PiezaPuzzle>();

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (!todasLasPiezas.Contains(this))
            todasLasPiezas.Add(this);
    }

    void OnDestroy()
    {
        todasLasPiezas.Remove(this);
    }

    void Start()
    {
        posicionInicial = rectTransform.anchoredPosition;
        rotacionInicial = rectTransform.rotation;

        if (usarPosicionAleatoria)
            AsignarPosicionAleatoriaLejana();

        if (usarRotacionAleatoria)
        {
            float angulo = Random.Range(0, 4) * 90f;
            rectTransform.rotation = Quaternion.Euler(0, 0, angulo);
        }
    }

    private void AsignarPosicionAleatoriaLejana()
    {
        RectTransform contenedor = rectTransform.parent as RectTransform;

        float anchoContenedor = contenedor.rect.width;
        float altoContenedor = contenedor.rect.height;
        float anchoPieza = rectTransform.rect.width;
        float altoPieza = rectTransform.rect.height;

        float xMin = -anchoContenedor / 2f + margenBordes.x + anchoPieza / 2f;
        float xMax = anchoContenedor / 2f - margenBordes.x - anchoPieza / 2f;
        float yMin = -altoContenedor / 2f + margenBordes.y + altoPieza / 2f;
        float yMax = altoContenedor / 2f - margenBordes.y - altoPieza / 2f;

        int intentos = 0;
        const int MAX_INTENTOS = 1000;
        bool posicionValida;
        Vector2 nuevaPosicion = Vector2.zero;

        do
        {
            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, yMax);
            nuevaPosicion = new Vector2(x, y);
            intentos++;

            // 1️⃣ Validar que no esté cerca de su propio objetivo
            posicionValida = Vector2.Distance(nuevaPosicion, ObtenerPosicionObjetivoLocal()) >= distanciaMinimaAlObjetivo;

            // 2️⃣ Validar que no esté cerca de los objetivos de otras piezas
            foreach (var otra in todasLasPiezas)
            {
                if (otra == this) continue;
                posicionValida &= Vector2.Distance(nuevaPosicion, otra.ObtenerPosicionObjetivoLocal()) >= distanciaMinimaAlObjetivo;
            }

            // 3️⃣ Validar que no se superponga con otras piezas
            if (posicionValida && evitarSolapamiento)
                posicionValida = !HaySolapamiento(nuevaPosicion);

        } while (!posicionValida && intentos < MAX_INTENTOS);

        rectTransform.anchoredPosition = nuevaPosicion;
    }

    private bool HaySolapamiento(Vector2 nuevaPos)
    {
        foreach (var otra in todasLasPiezas)
        {
            if (otra == this) continue;

            // Obtener tamaños de ambas piezas
            RectTransform rectOtra = otra.rectTransform;
            Vector2 sizeEsta = rectTransform.rect.size;
            Vector2 sizeOtra = rectOtra.rect.size;

            // Convertir posiciones locales a mundo para comparación
            Vector3 posEsta = rectTransform.parent.TransformPoint(nuevaPos);
            Vector3 posOtra = rectOtra.parent.TransformPoint(rectOtra.anchoredPosition);

            // Construir rectángulos (aproximation sin rotación)
            Rect rect1 = new Rect((Vector2)posEsta - sizeEsta / 2f, sizeEsta);
            Rect rect2 = new Rect((Vector2)posOtra - sizeOtra / 2f, sizeOtra);

            if (rect1.Overlaps(rect2))
                return true;
        }

        return false;
    }

    private Vector3 ObtenerPosicionObjetivoMundo()
    {
        // Devuelve la posición en el mundo del centro del objetivo
        return zonaObjetivo.TransformPoint(zonaObjetivo.rect.center);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling(); // Mantiene la pieza al frente
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Usamos posiciones en el mundo para medir distancia real
        Vector3 posicionPiezaMundo = rectTransform.TransformPoint(rectTransform.rect.center);
        Vector3 posicionObjetivoMundo = ObtenerPosicionObjetivoMundo();

        float distancia = Vector3.Distance(posicionPiezaMundo, posicionObjetivoMundo);

        // Medimos la diferencia de rotación correctamente
        float rotPieza = NormalizarAngulo(rectTransform.eulerAngles.z);
        float rotObjetivo = NormalizarAngulo(zonaObjetivo.eulerAngles.z);
        float diferenciaRot = Mathf.Abs(Mathf.DeltaAngle(rotPieza, rotObjetivo));
        bool rotacionCorrecta = diferenciaRot <= toleranciaRotacion;

        // Encaja solo si ambas condiciones se cumplen
        if (distancia < rangoSnap && rotacionCorrecta)
        {
            rectTransform.position = zonaObjetivo.position; // Igualamos posición mundial
            rectTransform.rotation = zonaObjetivo.rotation;
            this.enabled = false; // Fija la pieza
        }
        else
        {
            rectTransform.anchoredPosition = posicionInicial;
            rectTransform.rotation = rotacionInicial;
        }
    }

    public void ReiniciarEstado()
    {
        // Restablece la rotación inicial o aleatoria
        if (usarRotacionAleatoria)
        {
            float angulo = Random.Range(0, 4) * 90f;
            rectTransform.rotation = Quaternion.Euler(0, 0, angulo);
        }
        else
        {
            rectTransform.rotation = Quaternion.identity;
        }

        // Restablece la posición (aleatoria o inicial)
        if (usarPosicionAleatoria)
        {
            AsignarPosicionAleatoriaLejana();
            posicionInicial = rectTransform.anchoredPosition;
        }
        else
        {
            rectTransform.anchoredPosition = posicionInicial;
        }

        // Restaura el CanvasGroup si lo tenías modificado
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }


    private float NormalizarAngulo(float angulo)
    {
        angulo %= 360f;
        if (angulo < 0) angulo += 360f;
        return angulo;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (permitirRotacionManual && eventData.button == PointerEventData.InputButton.Right)
        {
            rectTransform.Rotate(0, 0, incrementoRotacion);
        }
    }

    private Vector2 ObtenerPosicionObjetivoLocal()
    {
        // Mantiene compatibilidad con la distancia mínima al generar aleatoriamente
        Vector2 posLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null, zonaObjetivo.position),
            canvas.worldCamera,
            out posLocal
        );
        return posLocal;
    }
}
