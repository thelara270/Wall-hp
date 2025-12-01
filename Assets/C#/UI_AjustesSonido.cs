using UnityEngine;
using UnityEngine.UI;

public class UI_AjustesSonido : MonoBehaviour
{
    public Slider sliderVolumenGeneral; // Referencia al slider que controla el volumen general
    public Slider sliderVolumenMusica;  // Referencia al slider que controla el volumen de música
    public Slider sliderVolumenSFX;     // Referencia al slider que controla los efectos de sonido

    void Start()
    {
        // Verifica que exista una instancia del AudioManager
        if (AudioManager.instance == null) return;

        // Asigna al slider de volumen general el valor guardado o 1 por defecto
        sliderVolumenGeneral.value = PlayerPrefs.GetFloat("generalVolume", 1f);
        // Asigna al slider de música el valor guardado o 1 por defecto
        sliderVolumenMusica.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        // Asigna al slider de efectos sonoros el valor guardado o 1 por defecto
        sliderVolumenSFX.value = PlayerPrefs.GetFloat("sfxVolume", 1f);

        // Escucha los cambios de valor en el slider de volumen general y ejecuta el método correspondiente
        sliderVolumenGeneral.onValueChanged.AddListener(SetGeneralVolume);
        // Escucha cambios en el slider de música usando el método de AudioManager
        sliderVolumenMusica.onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        // Escucha cambios en el slider de efectos usando el método de AudioManager
        sliderVolumenSFX.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
    }

    void SetGeneralVolume(float value)
    {
        // Aplica el volumen general a todo el sistema de audio
        AudioListener.volume = value;
        // Guarda el valor en PlayerPrefs para que se mantenga entre escenas y ejecuciones
        PlayerPrefs.SetFloat("generalVolume", value);
    }

    public void ActualizarSlidersAlMostrar()
    {
        // Este método puede llamarse cuando se abre el panel de sonido
        // para cargar nuevamente los valores actuales guardados
        sliderVolumenGeneral.value = PlayerPrefs.GetFloat("generalVolume", 1f);
        sliderVolumenMusica.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        sliderVolumenSFX.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }
}
