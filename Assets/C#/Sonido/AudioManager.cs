using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;   // Música
    public AudioSource sfxSource;     // Efectos puntuales (OneShot)
    public AudioSource loopSource;    // Sonidos en loop

    [Header("Música")]
    public AudioClip musicaMenu;
    public AudioClip musicaCinematica;
    public AudioClip musicaJuego;

    [Header("Efectos")]
    public AudioClip caminar;
    public AudioClip tomarObjeto;
    public AudioClip panelElectrico;
    public AudioClip panelCarpetas;
    public AudioClip regarPlanta;
    public AudioClip recargarAgua;
    public AudioClip sembrar;
    public AudioClip servidores;
    public AudioClip digitarCodigo;
    public AudioClip rompecabezasColocar;
    public AudioClip efectoPuertas;

    [Header("Click")]
    public AudioClip uiHover;
    public AudioClip uiClick;

    [Header("Enemigos")]
    public AudioClip sirenaEnemigo;
    public AudioClip explosionEnemigo;
    public AudioClip detectarTorreta;
    public AudioClip dispararTorreta;

    [Header("EVA")]
    public AudioClip laserPequeño;
    public AudioClip laserGrande;
    public AudioClip caerBrazo;
    public AudioClip pegarEva;
    public AudioClip electricidadEva;

    float musicVolume = 1f;
    float sfxVolume = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        ApplyVolume();
    }

    // ---------------- MÚSICA ---------------------

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void MusicaMenu() => PlayMusic(musicaMenu, true);
    public void MusicaCinematica() => PlayMusic(musicaCinematica, false);
    public void MusicaJuego() => PlayMusic(musicaJuego, true);

    // ---------------- SFX -------------------------

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // ---------------- LOOP ------------------------

    public void PlayLoop(AudioClip clip)
    {
        if (clip == null || loopSource == null) return;

        if (loopSource.clip != clip)
            loopSource.clip = clip;

        loopSource.volume = sfxVolume;

        if (!loopSource.isPlaying)
            loopSource.Play();
    }

    public void StopLoop()
    {
        if (loopSource != null && loopSource.isPlaying)
            loopSource.Stop();
    }

    // -------------- ACCESOS DIRECTOS --------------

    public void SonidoCaminar(bool activo)
    {
        if (activo) PlayLoop(caminar);
        else StopLoop();
    }

    public void SonidoTomarObjeto() => PlaySFX(tomarObjeto);
    public void SonidoPanelElectrico() => PlaySFX(panelElectrico);
    public void SonidoPanelCarpetas() => PlaySFX(panelCarpetas);
    public void SonidoRegarPlanta() => PlaySFX(regarPlanta);
    public void SonidoRecargarAgua() => PlaySFX(recargarAgua);
    public void SonidoSembrar() => PlaySFX(sembrar);
    public void SonidoServidores() => PlayLoop(servidores);
    public void DetenerServidores() => StopLoop();
    public void SonidoDigitarCodigo() => PlaySFX(digitarCodigo);
    public void SonidoRompecabezas() => PlaySFX(rompecabezasColocar);
    public void SonidoPuertas() => PlaySFX(efectoPuertas);

    // ----------- Enemigos ----------------------
    public void SonidoSirenaEnemigo() => PlaySFX(sirenaEnemigo);
    public void SonidoExplosionEnemigo() => PlaySFX(explosionEnemigo);
    public void SonidoDetectarTorreta() => PlaySFX(detectarTorreta);
    public void SonidoDispararTorreta() => PlaySFX(dispararTorreta);

    // ----------- EVA ----------------------
    public void SonidoLaserPequeño() => PlaySFX(laserPequeño);
    public void SonidoLaserGrande() => PlaySFX(laserGrande);
    public void SonidoCaerBrazo() => PlaySFX(caerBrazo);
    public void SonidoPegarEva() => PlaySFX(pegarEva);
    public void SonidoElectricidadEva() => PlaySFX(electricidadEva);

    // ----------- UI BUTTONS ----------------------

    public void SonidoHoverUI() => PlaySFX(uiHover);
    public void SonidoClickUI() => PlaySFX(uiClick);

    // ------------ SLIDERS DE VOLUMEN ---------------

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = sfxVolume;
        loopSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    void ApplyVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;

        if (loopSource != null)
            loopSource.volume = sfxVolume;
    }

    public void PararTodo()
    {
        if (musicSource != null)
            musicSource.Stop();

        if (sfxSource != null)
            sfxSource.Stop();

        if (loopSource != null)
            loopSource.Stop();
    }
}
