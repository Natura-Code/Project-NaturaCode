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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        mainBackgroundSource = GameObject.FindWithTag("Music")?.GetComponent<AudioSource>();
        ambientSource = GameObject.FindWithTag("Ambient")?.GetComponent<AudioSource>();
        sfxSource = GameObject.FindWithTag("SFX")?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitializeSliders();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        if (musicSlider == null)
            musicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
        if (ambientSlider == null)
            ambientSlider = GameObject.Find("AmbientSlider")?.GetComponent<Slider>();
        if (sfxSlider == null)
            sfxSlider = GameObject.Find("SFXSlider")?.GetComponent<Slider>();

        if (musicSlider == null || ambientSlider == null || sfxSlider == null)
        {
            Debug.LogError("Slider not found! Make sure sliders are in the scene and properly connected.");
            return;
        }

        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float savedAmbientVolume = PlayerPrefs.GetFloat(AmbientVolumeKey, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        ApplyVolume(mainBackgroundSource, savedMusicVolume);
        ApplyVolume(ambientSource, savedAmbientVolume);
        ApplyVolume(sfxSource, savedSFXVolume);

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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
