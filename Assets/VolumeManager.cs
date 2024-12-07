using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider sfxSlider;

    private AudioSource mainBackgroundSource;
    private AudioSource ambientSource;
    private AudioSource sfxSource;

    private const string MusicVolumeKey = "MusicVolume";
    private const string AmbientVolumeKey = "AmbientVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Find AudioSources by tag
        mainBackgroundSource = GameObject.FindWithTag("Music")?.GetComponent<AudioSource>();
        ambientSource = GameObject.FindWithTag("Ambient")?.GetComponent<AudioSource>();
        sfxSource = GameObject.FindWithTag("SFX")?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Initialize sliders when the game starts
        InitializeSliders();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinitialize sliders after scene is loaded
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        // Coba mencari slider jika belum terhubung di Inspector
        if (musicSlider == null)
            musicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
        if (ambientSlider == null)
            ambientSlider = GameObject.Find("AmbientSlider")?.GetComponent<Slider>();
        if (sfxSlider == null)
            sfxSlider = GameObject.Find("SFXSlider")?.GetComponent<Slider>();

        // Pastikan slider ditemukan
        if (musicSlider == null || ambientSlider == null || sfxSlider == null)
        {
            Debug.LogError("Slider not found! Make sure sliders are in the scene and properly connected.");
            return;
        }

        // Load saved volume settings
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float savedAmbientVolume = PlayerPrefs.GetFloat(AmbientVolumeKey, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        // Apply volumes to AudioSources
        ApplyVolume(mainBackgroundSource, savedMusicVolume);
        ApplyVolume(ambientSource, savedAmbientVolume);
        ApplyVolume(sfxSource, savedSFXVolume);

        // Initialize sliders with saved volume
        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        if (ambientSlider != null)
        {
            ambientSlider.value = savedAmbientVolume;
            ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        ApplyVolume(mainBackgroundSource, volume);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetAmbientVolume(float volume)
    {
        ApplyVolume(ambientSource, volume);
        PlayerPrefs.SetFloat(AmbientVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        ApplyVolume(sfxSource, volume);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(AudioSource source, float volume)
    {
        if (source != null)
        {
            source.volume = volume;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
