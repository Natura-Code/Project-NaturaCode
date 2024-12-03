using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator anime;

    private Vector2 movement;
    private Rigidbody2D rb;

    private GoldManager goldManager;
    private bool isStunned = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Load posisi pemain untuk scene "InGameSea" jika tersedia
        if (PlayerPrefs.HasKey("InGameSea_X") && PlayerPrefs.HasKey("InGameSea_Y"))
        {
            float x = PlayerPrefs.GetFloat("InGameSea_X");
            float y = PlayerPrefs.GetFloat("InGameSea_Y");
            transform.position = new Vector3(x, y, 0);
        }

        goldManager = FindObjectOfType<GoldManager>();
        if (goldManager == null)
        {
            Debug.LogError("GoldManager tidak ditemukan");
        }
    }

    private void Update()
    {
        if (isStunned) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        FlipAnimation();

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

    private void FlipAnimation()
    {
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
        else
        {
            anime.SetBool("Swim", false);
            anime.SetBool("SwimX", false);
            anime.SetBool("SwimY", false);
        }
    }

    private void TryCatchFish()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (var obj in nearbyObjects)
        {
            if (obj.CompareTag("Fish"))
            {
                if (goldManager != null)
                {
                    goldManager.ChangeGold(10); // Tambah 10 gold
                }

                Destroy(obj.gameObject); // Hancurkan ikan
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f); // Visual radius tangkapan
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        Debug.Log("Player stunned!");
        isStunned = true;

        yield return new WaitForSeconds(duration);

        Debug.Log("Player recovered from stun.");
        isStunned = false;
    }

    private void OnApplicationQuit()
    {
        // Simpan posisi pemain untuk scene "InGameSea" sebelum keluar
        PlayerPrefs.SetFloat("InGameSea_X", transform.position.x);
        PlayerPrefs.SetFloat("InGameSea_Y", transform.position.y);
        PlayerPrefs.Save();
    }
}
