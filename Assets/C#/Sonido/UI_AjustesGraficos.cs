using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_AjustesGraficos : MonoBehaviour
{
    public TMP_Dropdown dropdownResoluciones; // Referencia al Dropdown (TextMeshPro)
    public Toggle togglePantallaCompleta;     // Referencia al Toggle de pantalla completa

    private Resolution[] resolucionesOriginales; // Todas las resoluciones que da Unity
    private List<Resolution> resolucionesFiltradas; // Resoluciones sin duplicados
    private bool cargado = false; // Bandera para evitar duplicaciones

    void Awake()
    {
        // Si ya se cargo previamente, salimos para evitar cargado repetido
        if (cargado) return;
        cargado = true;

        // Obtenemos todas las resoluciones originales del sistema
        resolucionesOriginales = Screen.resolutions;

        // Inicializamos la lista de resoluciones filtradas
        resolucionesFiltradas = new List<Resolution>();

        // Filtramos resoluciones duplicadas basadas solo en ancho y alto
        foreach (Resolution res in resolucionesOriginales)
        {
            // Verificamos si ya hay una resolucion con el mismo width y height
            if (!resolucionesFiltradas.Exists(r => r.width == res.width && r.height == res.height))
            {
                resolucionesFiltradas.Add(res);
            }
        }

        // Ordenamos las resoluciones de mayor a menor para que las mas comunes aparezcan primero
        resolucionesFiltradas.Sort((a, b) => (b.width * b.height).CompareTo(a.width * a.height));

        // Eliminamos opciones previas del dropdown para evitar que se acumulen
        dropdownResoluciones.ClearOptions();

        // Creamos lista de opciones para el dropdown
        List<string> opciones = new List<string>();

        // Variable para guardar el indice de la resolucion actual
        int indiceResolucionActual = 0;

        // Recorremos las resoluciones filtradas
        for (int i = 0; i < resolucionesFiltradas.Count; i++)
        {
            // Formato de la resolucion que se mostrara en el dropdown
            string opcion = resolucionesFiltradas[i].width + " x " + resolucionesFiltradas[i].height;
            opciones.Add(opcion);

            // Verificamos si esta es la resolucion actual
            if (resolucionesFiltradas[i].width == Screen.currentResolution.width &&
                resolucionesFiltradas[i].height == Screen.currentResolution.height)
            {
                indiceResolucionActual = i;
            }
        }

        // Agregamos las opciones al dropdown
        dropdownResoluciones.AddOptions(opciones);

        // Establecemos la resolucion actual como seleccionada
        dropdownResoluciones.value = indiceResolucionActual;
        dropdownResoluciones.RefreshShownValue();

        // Quitamos listeners previos para evitar que se dupliquen
        dropdownResoluciones.onValueChanged.RemoveAllListeners();
        togglePantallaCompleta.onValueChanged.RemoveAllListeners();

        // Agregamos los nuevos listeners
        dropdownResoluciones.onValueChanged.AddListener(CambiarResolucion);
        togglePantallaCompleta.onValueChanged.AddListener(CambiarModoPantalla);
    }

    void Start()
    {
        // Si NO existe un valor guardado, significa que es la primera ejecución
        if (!PlayerPrefs.HasKey("resolucionIndex"))
        {
            int indice = 0; // Índice por defecto

            // Recorremos las resoluciones filtradas para buscar 1920x1080
            for (int i = 0; i < resolucionesFiltradas.Count; i++)
            {
                if (resolucionesFiltradas[i].width == 1920 && resolucionesFiltradas[i].height == 1080)
                {
                    indice = i; // Si encontramos 1920x1080, la usamos
                    break;
                }
            }

            // Si no existe 1920x1080, usamos la resolución más alta (última del listado)
            if (resolucionesFiltradas.Count > 0 && (resolucionesFiltradas[indice].width < 1920 || resolucionesFiltradas[indice].height < 1080))
            {
                indice = resolucionesFiltradas.Count - 1; // Última resolución (mayor normalmente)
            }

            // Aplicamos la resolución y modo de pantalla inicial
            dropdownResoluciones.value = indice;
            togglePantallaCompleta.isOn = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(resolucionesFiltradas[indice].width, resolucionesFiltradas[indice].height, true);

            // Guardamos parámetros para que no vuelva a entrar aquí
            PlayerPrefs.SetInt("resolucionIndex", indice);
            PlayerPrefs.SetInt("pantallaCompleta", 1);
            PlayerPrefs.Save();

            Debug.Log("Primera ejecución → Resolución establecida en: " +
                      resolucionesFiltradas[indice].width + "x" +
                      resolucionesFiltradas[indice].height + " | Pantalla completa: true");
        }
        else
        {
            // Si ya hay configuración guardada, solo la cargamos
            dropdownResoluciones.value = PlayerPrefs.GetInt("resolucionIndex");
            togglePantallaCompleta.isOn = PlayerPrefs.GetInt("pantallaCompleta") == 1;
            dropdownResoluciones.RefreshShownValue();
        }
    }

    void EstablecerResolucionInicial()
    {
        // Buscamos la resolucion 1920x1080 en la lista filtrada
        int indice = 0;
        for (int i = 0; i < resolucionesFiltradas.Count; i++)
        {
            if (resolucionesFiltradas[i].width == 1920 && resolucionesFiltradas[i].height == 1080)
            {
                indice = i;
                break;
            }
        }

        // Establecemos el valor en el dropdown
        dropdownResoluciones.value = indice;

        // Activamos pantalla completa
        togglePantallaCompleta.isOn = true;

        // Aplicamos la resolucion
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(1920, 1080, true);

        // Guardamos los valores
        PlayerPrefs.SetInt("resolucionIndex", indice);
        PlayerPrefs.SetInt("pantallaCompleta", 1);
        PlayerPrefs.Save();
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        // Obtenemos la resolucion seleccionada
        Resolution resolucion = resolucionesFiltradas[indiceResolucion];

        // Verificamos si debe estar en pantalla completa
        bool pantallaCompleta = togglePantallaCompleta.isOn;

        // Aplicamos la resolucion
        Screen.SetResolution(resolucion.width, resolucion.height, pantallaCompleta);

        // Guardamos el valor
        PlayerPrefs.SetInt("resolucionIndex", indiceResolucion);
        PlayerPrefs.Save();

        Debug.Log("Resolucion cambiada a: " + resolucion.width + "x" + resolucion.height +
                  " | Pantalla completa: " + pantallaCompleta);
    }

    public void CambiarModoPantalla(bool pantallaCompleta)
    {
        // Cambiamos el modo de pantalla
        Screen.fullScreenMode = pantallaCompleta ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        // Reaplicamos la resolucion actual
        Screen.SetResolution(Screen.width, Screen.height, pantallaCompleta);

        // Guardamos el estado
        PlayerPrefs.SetInt("pantallaCompleta", pantallaCompleta ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Modo de pantalla: " + pantallaCompleta);
    }
}
