using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string colorCable;                //Identificador del color

    private CanvasGroup canvasGroup;         //Referencia al CanvasGroup para controlar interacciones
    private RectTransform rectTransform;     //Referencia al RectTransform para posicion y tamaño
    private Transform padreOriginal;         //Guarda el padre original para regresar al soltar

    private Vector3 posicionInicialLocal;    //posicion local dentro del padre original
    private Vector3 posicionInicialWorld;    //posicion en el mundo para conversiones

    public UILineRenderer line;              //Componente para dibujar la linea del cable

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();  //Obtener componente RectTransform
        canvasGroup = GetComponent<CanvasGroup>();      //Obtener componente CanvasGroup

        padreOriginal = transform.parent;                   //Guardar padre original
        posicionInicialLocal = rectTransform.localPosition; //Guardar posicion local inicial
        posicionInicialWorld = rectTransform.position;      //Guardar posicion world inicial
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;   //Permitir pasar raycasts para detectar drop
        transform.SetParent(transform.root);  //Poner el objeto en la raiz para que quede encima de todo
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position; //Mover cable al puntero del mouse/touch

        RectTransform lineRect = line.rectTransform; //Obtener rectTransform del componente line

        //Convertir posicion inicial world a pantalla
        Vector2 screenPosInicial = RectTransformUtility.WorldToScreenPoint(null, posicionInicialWorld);

        //Convertir puntos de pantalla a local dentro del rect que dibuja la linea
        Vector2 localStart, localEnd;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineRect, screenPosInicial, null, out localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(lineRect, eventData.position, null, out localEnd);

        //Actualizar puntos para dibujar la linea entre inicial y el puntero
        line.SetPoints(new List<Vector2> { localStart, localEnd });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; //Volver a bloquear raycasts

        if (transform.parent == transform.root) //Si no se solto en un padre valido
        {
            transform.SetParent(padreOriginal); //Regresar a padre original
            rectTransform.localPosition = posicionInicialLocal; //Regresar a posicion inicial local
            line.SetPoints(null); //Limpiar linea dibujada
        }
    }
}
