using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator anime;
    [SerializeField] private AudioClip walkSFX; 

    private AudioSource sfxSource; 
    private Vector3 movement;

    private void Start()
    {
        if (PlayerPrefs.HasKey("InGame_PosX") && PlayerPrefs.HasKey("InGame_PosY"))
        {
            float x = PlayerPrefs.GetFloat("InGame_PosX");
            float y = PlayerPrefs.GetFloat("InGame_PosY");
            transform.position = new Vector3(x, y, transform.position.z);
        }

        sfxSource = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();

        if (sfxSource == null)
        {
            Debug.LogError("AudioSource tidak ditemukan pada objek dengan tag SFX.");
        }

        sfxSource.loop = true;
        sfxSource.clip = walkSFX;
    }

    private void Update()
    {
        movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movement.y = 1;
        else if (Input.GetKey(KeyCode.S)) movement.y = -1;
        else if (Input.GetKey(KeyCode.A)) movement.x = -1;
        else if (Input.GetKey(KeyCode.D)) movement.x = 1;

        transform.position += movement * speed * Time.deltaTime;

        HandleAnimationAndSFX();
    }

    private void HandleAnimationAndSFX()
    {
        if (movement != Vector3.zero)
        {
            if (!sfxSource.isPlaying) sfxSource.Play();

            if (movement.x != 0)
            {
                anime.SetBool("Jalan", true);
                anime.SetBool("JalanZ", false);
                anime.SetBool("JalanY", false);
                transform.localScale = new Vector3(movement.x < 0 ? -1 : 1, 1, 1);
            }
            else if (movement.y > 0)
            {
                anime.SetBool("JalanZ", true);
                anime.SetBool("JalanY", false);
                anime.SetBool("Jalan", false);
            }
            else if (movement.y < 0)
            {
                anime.SetBool("JalanY", true);
                anime.SetBool("JalanZ", false);
                anime.SetBool("Jalan", false);
            }
        }
        else
        {
            sfxSource.Stop();

            anime.SetBool("Jalan", false);
            anime.SetBool("JalanY", false);
            anime.SetBool("JalanZ", false);
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("InGame_PosX", transform.position.x);
        PlayerPrefs.SetFloat("InGame_PosY", transform.position.y);
        PlayerPrefs.Save();
    }
}
