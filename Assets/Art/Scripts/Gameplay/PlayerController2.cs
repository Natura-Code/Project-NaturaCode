using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;
    [SerializeField] private Animator anime;

    private GoldManager goldManager;
    private bool isStunned = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;

        goldManager = FindObjectOfType<GoldManager>();
        if (goldManager == null)
        {
            Debug.LogError("GoldManager tidak ditemukan");
        }
    }

    void Update()
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

            if (movement.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (movement.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (movement.y > 0)
        {
            anime.SetBool("Swim", true);
            anime.SetBool("SwimX", false);
            anime.SetBool("SwimY", false);
        }
        else if (movement.y < 0)
        {
            anime.SetBool("Swim", false);
            anime.SetBool("SwimX", false);
            anime.SetBool("SwimY", true);
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
                    goldManager.ChangeGold(10); 
                }

                Destroy(obj.gameObject); 
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f); 
    }
    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        Debug.Log("Player stunned!");
        enabled = false;

        yield return new WaitForSeconds(duration);

        Debug.Log("Player recovered from stun.");
        enabled = true;
    }
}
