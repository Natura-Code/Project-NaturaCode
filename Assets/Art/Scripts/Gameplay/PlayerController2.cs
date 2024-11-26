using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private bool isStunned = false; // Apakah pemain sedang terkena stun

    [SerializeField] private float speed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;

    [SerializeField] private Animator anime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
    }

    void Update()
    {
        if (isStunned) return; // Abaikan input jika sedang stun

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        FlipAnimation();
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

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        Debug.Log("Player stunned!");
        // Disable movement
        enabled = false;

        yield return new WaitForSeconds(duration);

        // Enable movement
        Debug.Log("Player recovered from stun.");
        enabled = true;
    }



}
