using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class AutoResizeTextBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Vector2 padding = new Vector2(20, 20);
    public float maxWidth = 400f;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (textComponent == null) return;

        // Forzar actualización del layout del texto
        textComponent.ForceMeshUpdate();

        // Obtener tamaño preferido del texto
        Vector2 textSize = textComponent.GetPreferredValues(textComponent.text, maxWidth, 0);

        // Ajustar tamaño del cuadro sumando padding
        rectTransform.sizeDelta = new Vector2(
            Mathf.Min(textSize.x + padding.x, maxWidth),
            textSize.y + padding.y
        );
    }
}