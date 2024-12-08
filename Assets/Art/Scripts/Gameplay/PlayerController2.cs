using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator anime;
    [SerializeField] private AudioClip swimSFX;
    [SerializeField] private AudioClip stunSFX;
    [SerializeField] private AudioClip catchFishSFX;

    [Header("Item Management")]
    [SerializeField] private Item fishItem; 
    [SerializeField] private InventoryManager inventoryManager; 
    private AudioSource sfxSource;
    private Vector2 movement;
    private Rigidbody2D rb;
    private bool isStunned = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (PlayerPrefs.HasKey("InGameSea_X") && PlayerPrefs.HasKey("InGameSea_Y"))
        {
            float x = PlayerPrefs.GetFloat("InGameSea_X");
            float y = PlayerPrefs.GetFloat("InGameSea_Y");
            transform.position = new Vector3(x, y, 0);
        }

        sfxSource = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        if (sfxSource == null)
        {
            Debug.LogError("AudioSource tidak ditemukan pada objek dengan tag SFX.");
        }

        sfxSource.loop = true;
        sfxSource.clip = swimSFX;
    }

    private void Update()
    {
        if (isStunned) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        HandleAnimationAndSFX();

        if (Input.GetMouseButtonDown(0))
        {
            TryCatchFish();
        }
    }

    private void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            rb.gravityScale = 0;
            rb.velocity = movement.normalized * speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 10;
        }
    }

    private void HandleAnimationAndSFX()
    {
        if (movement != Vector2.zero)
        {
            if (!sfxSource.isPlaying) sfxSource.Play();

            if (movement.x != 0)
            {
                anime.SetBool("SwimX", true);
                anime.SetBool("SwimY", false);
                anime.SetBool("Swim", false);
                transform.localScale = new Vector3(movement.x < 0 ? -1 : 1, 1, 1);
            }
            else if (movement.y > 0)
            {
                anime.SetBool("Swim", true);
                anime.SetBool("SwimX", false);
                anime.SetBool("SwimY", false);
            }
            else if (movement.y < 0)
            {
                anime.SetBool("SwimY", true);
                anime.SetBool("SwimX", false);
                anime.SetBool("Swim", false);
            }
        }
        else
        {
            sfxSource.Stop();

            anime.SetBool("Swim", false);
            anime.SetBool("SwimX", false);
            anime.SetBool("SwimY", false);
        }
    }

    private void TryCatchFish()
    {
        try
        {
            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (var obj in nearbyObjects)
            {
                if (obj.CompareTag("Fish"))
                {
                    if (inventoryManager != null && fishItem != null)
                    {
                        bool added = inventoryManager.AddItem(fishItem);

                        if (added)
                        {
                            Debug.Log("Item added: " + fishItem.name);
                            PlaySFX(catchFishSFX);
                            Destroy(obj.gameObject); 
                        }
                        else
                        {
                            Debug.Log("Inventory full! Cannot add item: " + fishItem.name);
                        }
                    }
                    else
                    {
                        Debug.LogError("InventoryManager atau fishItem belum diatur.");
                    }

                    break;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saat mencoba menangkap ikan: " + ex.Message);
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void Stun(float stunDuration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(stunDuration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        PlaySFX(stunSFX);

        yield return new WaitForSeconds(duration);

        isStunned = false;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("InGameSea_X", transform.position.x);
        PlayerPrefs.SetFloat("InGameSea_Y", transform.position.y);
        PlayerPrefs.Save();
    }
}
