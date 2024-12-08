using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private AudioSource mainBackgroundSource; 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Clip")]
    public AudioClip MainMenuSound;      
    public AudioClip mainBackgroundClip; 
    public AudioClip backgroundInGame;   
    public AudioClip backgroundInGame2;  

    private void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); 
    }

    private void Start()
    {
        UpdateSceneMusic(SceneManager.GetActiveScene().name);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateSceneMusic(scene.name);
    }

    private void UpdateSceneMusic(string sceneName)
    {
        mainBackgroundSource.Stop();
        musicSource.Stop();

        switch (sceneName)
        {
            case "MainMenu":
                mainBackgroundSource.clip = MainMenuSound;
                mainBackgroundSource.loop = true;
                mainBackgroundSource.Play();
                break;

            case "InGame":
                mainBackgroundSource.clip = mainBackgroundClip;
                mainBackgroundSource.loop = true;
                mainBackgroundSource.Play();

                musicSource.clip = backgroundInGame;
                musicSource.loop = true;
                musicSource.Play();
                break;

            case "InGameSea":
                mainBackgroundSource.clip = mainBackgroundClip;
                mainBackgroundSource.loop = true;
                mainBackgroundSource.Play();

                musicSource.clip = backgroundInGame2;
                musicSource.loop = true;
                musicSource.Play();
                break;

            default:
                mainBackgroundSource.clip = null;
                musicSource.clip = null;
                break;
        }
    }
}