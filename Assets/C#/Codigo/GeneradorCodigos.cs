using UnityEngine;
using TMPro;
using System;

public class GeneradorCodigos : MonoBehaviour
{
    [Header("Configuración")]
    public int cantidadCaracteres = 6;
    public TMP_Text[] panelesCodigo;
    public TMP_InputField inputJugador;
    public TMP_Text resultadoTexto;

    private string codigoCorrecto;
    private int indiceCorrecto;
    private System.Random random;

    public event Action OnCodigoCorrecto;

    public ZonaCodigo zonaCodigo;

    private void Start()
    {
        random = new System.Random();
        Invoke(nameof(GenerarCodigos), 0.1f);
    }

    public void GenerarCodigos()
    {
        if (panelesCodigo == null || panelesCodigo.Length == 0)
        {
            Debug.LogError("No hay paneles asignados en el Inspector.");
            return;
        }

        codigoCorrecto = GenerarCodigoAleatorio(cantidadCaracteres);
        indiceCorrecto = random.Next(0, panelesCodigo.Length);

        Debug.Log($"[GeneradorCodigos] Código correcto: {codigoCorrecto} (Panel índice: {indiceCorrecto})");

        for (int i = 0; i < panelesCodigo.Length; i++)
        {
            panelesCodigo[i].text = (i == indiceCorrecto)
                ? codigoCorrecto
                : GenerarCodigoAleatorio(cantidadCaracteres);
        }

        resultadoTexto.text = "";
    }

    private string GenerarCodigoAleatorio(int longitud)
    {
        const string chars = "ABCDFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string codigo = "";

        for (int i = 0; i < longitud; i++)
            codigo += chars[random.Next(chars.Length)];

        return codigo;
    }

    public void VerificarCodigo()
    {
        if (inputJugador.text.ToUpper() == codigoCorrecto)
        {
            resultadoTexto.text = "Código correcto.";
            resultadoTexto.color = Color.green;

            AudioManager.instance?.SonidoDigitarCodigo();
            AudioManager.instance?.DetenerServidores();


            // ⏳ Cierre automático tras acierto
            Invoke(nameof(CerrarAutomaticamente), zonaCodigo.tiempoCierreAutomatico);

            OnCodigoCorrecto?.Invoke();
        }
        else
        {
            resultadoTexto.text = "Código incorrecto.";
            resultadoTexto.color = Color.red;

            AudioManager.instance?.SonidoServidores();
        }
    }

    private void CerrarAutomaticamente()
    {
        zonaCodigo.CerrarPanel();
    }

    public bool CodigoEsCorrecto()
    {
        return inputJugador.text.ToUpper() == codigoCorrecto;
    }
}
