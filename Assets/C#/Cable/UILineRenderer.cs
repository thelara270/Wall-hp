using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]                             //Hace que el script se ejecute incluso en modo editor
[RequireComponent(typeof(CanvasRenderer))]  //Obliga a tener un CanvasRenderer en el GameObject
public class UILineRenderer : MaskableGraphic
{
    public List<Vector2> puntosLinea = new List<Vector2>(); //Lista de puntos para dibujar la linea

    public Color colorLinea = Color.white;  //Color de la linea
    public float grosorLinea = 2f;          //Grosor de la linea
    public float fuerzaCurva = 50f;         //Fuerza de curvatura para curvas bezier
    public int segmentosLinea = 20;         //Cantidad de segmentos para cada curva

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear(); //Limpiar vertices anteriores

        if (puntosLinea == null || puntosLinea.Count < 2) //Si no hay puntos suficientes, no dibuja
            return;

        int vertexIndex = 0; //Indice para vertices

        for (int i = 1; i < puntosLinea.Count; i++) //Iterar entre pares de puntos
        {
            Vector2 p0 = puntosLinea[i - 1]; //Punto inicial
            Vector2 p2 = puntosLinea[i]; //Punto final

            Vector2 direction = (p2 - p0).normalized; //Direccion normalizada entre puntos
            Vector2 normal = new Vector2(-direction.y, direction.x); //Vector normal perpendicular
            Vector2 mid = (p0 + p2) / 2f; //Punto medio entre p0 y p2
            Vector2 control = mid + normal * fuerzaCurva; //Punto de control para curva bezier

            Vector2 lastPoint = p0; //Ultimo punto dibujado inicia en p0

            for (int j = 1; j <= segmentosLinea; j++) //Iterar segmentos para suavizar curva
            {
                float t = j / (float)segmentosLinea; //Parametro t de 0 a 1 para bezier
                Vector2 pointOnCurve = GetQuadraticBezierPoint(p0, control, p2, t); //Calcular punto en curva
                DrawLineSegment(vh, lastPoint, pointOnCurve, grosorLinea, colorLinea, ref vertexIndex); //Dibujar segmento de linea
                lastPoint = pointOnCurve; //Actualizar ultimo punto
            }
        }
    }

    private Vector2 GetQuadraticBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        //Formula para punto en curva cuadratica de bezier
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }

    private void DrawLineSegment(VertexHelper vh, Vector2 start, Vector2 end, float thickness, Color color, ref int index)
    {
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg; //Calcular angulo de segmento

        //Calcular vertices del rectangulo que forma la linea con grosor
        Vector2 v1 = start + new Vector2(0, -thickness / 2);
        Vector2 v2 = start + new Vector2(0, +thickness / 2);
        Vector2 v3 = end + new Vector2(0, +thickness / 2);
        Vector2 v4 = end + new Vector2(0, -thickness / 2);

        //Rotar vertices para que sigan la direccion del segmento
        v1 = RotatePointAroundPivot(v1, start, angle);
        v2 = RotatePointAroundPivot(v2, start, angle);
        v3 = RotatePointAroundPivot(v3, end, angle);
        v4 = RotatePointAroundPivot(v4, end, angle);

        AddQuad(vh, v1, v2, v3, v4, color, ref index); //Agregar el quad al mesh
    }

    private void AddQuad(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color color, ref int index)
    {
        UIVertex vert = UIVertex.simpleVert; //Crear vertice simple
        vert.color = color; //Asignar color

        vert.position = v1; vh.AddVert(vert); //Agregar vertice 1
        vert.position = v2; vh.AddVert(vert); //Agregar vertice 2
        vert.position = v3; vh.AddVert(vert); //Agregar vertice 3
        vert.position = v4; vh.AddVert(vert); //Agregar vertice 4

        //Agregar triangulos para formar el quad
        vh.AddTriangle(index, index + 1, index + 2);
        vh.AddTriangle(index + 2, index + 3, index);

        index += 4; //Incrementar indice para siguientes vertices
    }

    private Vector2 RotatePointAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        Vector2 dir = point - pivot; //Vector desde pivote al punto
        dir = Quaternion.Euler(0, 0, angle) * dir; //Rotar vector
        return pivot + dir; //Retornar nuevo punto rotado
    }

    //Metodo publico para actualizar puntos y forzar redibujado
    public void SetPoints(List<Vector2> points)
    {
        puntosLinea = points; //Asignar nueva lista de puntos
        SetVerticesDirty(); //Marcar mesh para redibujar
    }
}
