using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private AudioSource mainBackgroundSource; 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Clip")]
    public AudioClip MainMenuSound;      // Musik untuk MainMenu
    public AudioClip mainBackgroundClip; // Musik utama yang diputar di semua scene selain MainMenu
    public AudioClip backgroundInGame;   // Musik tambahan untuk scene InGame
    public AudioClip backgroundInGame2;  // Musik tambahan untuk scene InGameSea

    private void Awake()
    {
        // Pastikan hanya ada satu instance AudioManager
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Jangan hancurkan saat berpindah scene
    }

    private void Start()
    {
        // Atur musik berdasarkan scene saat ini
        UpdateSceneMusic(SceneManager.GetActiveScene().name);

        // Menambahkan handler untuk event saat scene dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Hapus handler event saat objek dihancurkan
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Perbarui musik setiap kali scene baru dimuat
        UpdateSceneMusic(scene.name);
    }

    private void UpdateSceneMusic(string sceneName)
    {
        // Hentikan semua audio sebelum memperbarui
        mainBackgroundSource.Stop();
        musicSource.Stop();

        switch (sceneName)
        {
            case "MainMenu":
                // Putar hanya MainMenuSound
                mainBackgroundSource.clip = MainMenuSound;
                mainBackgroundSource.loop = true;
                mainBackgroundSource.Play();
                break;

            case "InGame":
                // Putar mainBackgroundClip dan backgroundInGame
                mainBackgroundSource.clip = mainBackgroundClip;
                mainBackgroundSource.loop = true;
                mainBackgroundSource.Play();

                musicSource.clip = backgroundInGame;
                musicSource.loop = true;
                musicSource.Play();
                break;

            case "InGameSea":
                // Putar mainBackgroundClip dan backgroundInGame2
                mainBackgroundSource.clip = mainBackgroundClip;
                mainBackgroundSource.loop = true;
                mainBackgroundSource.Play();

                musicSource.clip = backgroundInGame2;
                musicSource.loop = true;
                musicSource.Play();
                break;

            default:
                // Tidak ada musik untuk scene lain
                mainBackgroundSource.clip = null;
                musicSource.clip = null;
                break;
        }
    }
}