using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private AudioSource mainBackgroundSource; // Untuk MainBackground
    [SerializeField] private AudioSource musicSource;          // Untuk musik per scene
    [SerializeField] private AudioSource SFXSource;            // Untuk efek suara (opsional)

    [Header("Clip")]
    public AudioClip mainBackgroundClip;       // Musik utama (MainBackground)
    public AudioClip backgroundInGame;         // Musik khusus untuk scene InGame
    public AudioClip backgroundInGame2;        // Musik khusus untuk scene InGameSea

    private void Awake()
    {
        // Pastikan hanya ada satu instance AudioManager
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Pertahankan AudioManager di semua scene
    }

    private void Start()
    {
        // Mainkan MainBackground saat game dimulai
        mainBackgroundSource.clip = mainBackgroundClip;
        mainBackgroundSource.loop = true;
        if (!mainBackgroundSource.isPlaying) mainBackgroundSource.Play();

        // Mainkan musik awal sesuai dengan scene pertama
        UpdateSceneMusic(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded; // Listener untuk event sceneLoaded
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Lepas listener saat AudioManager dihancurkan
    }

    // Callback untuk event sceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateSceneMusic(scene.name); // Perbarui musik berdasarkan nama scene
    }

    // Metode untuk mengganti musik berdasarkan nama scene
    private void UpdateSceneMusic(string sceneName)
    {
        switch (sceneName)
        {
            case "InGame":
                musicSource.clip = backgroundInGame;
                break;

            case "InGameSea":
                musicSource.clip = backgroundInGame2;
                break;

            default:
                musicSource.clip = null;
                break;
        }

        if (musicSource.clip != null)
        {
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
