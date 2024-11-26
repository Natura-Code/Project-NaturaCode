using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishAI : MonoBehaviour
{
    [Header("Player Interaction")]
    public Transform player; // Objek pemain
    public float chaseRadius = 5f; // Jarak untuk mulai mengejar
    public float stingRadius = 1.5f; // Jarak untuk menyengat
    public float fleeDistance = 3f; // Jarak untuk menjauh setelah menyengat

    [Header("Speeds")]
    public float chaseSpeed = 3f; // Kecepatan mengejar
    public float fleeSpeed = 4f; // Kecepatan menjauh

    [Header("Sting Settings")]
    public float stingDuration = 2f; // Durasi stun pemain
    public float fleeAfterStingTime = 1f; // Waktu sebelum mulai menjauh

    private Rigidbody2D rb; // Untuk menggerakkan ubur-ubur
    private Animator animator; // Untuk animasi
    private bool isStinging = false; // Apakah sedang menyengat?

    private enum State { Idle, Chasing, Stinging, Fleeing }
    private State currentState = State.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isStinging) return; // Jangan ubah state saat menyengat

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer <= chaseRadius)
                {
                    currentState = State.Chasing;
                }
                break;

            case State.Chasing:
                if (distanceToPlayer <= stingRadius)
                {
                    StartCoroutine(StingPlayer());
                }
                else if (distanceToPlayer > chaseRadius)
                {
                    currentState = State.Idle;
                }
                break;

            case State.Fleeing:
                if (distanceToPlayer >= fleeDistance)
                {
                    currentState = State.Idle;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (isStinging) return;

        switch (currentState)
        {
            case State.Chasing:
                ChasePlayer();
                break;

            case State.Fleeing:
                FleeFromPlayer();
                break;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
        UpdateJellyfishDirection(direction);
    }

    void FleeFromPlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        rb.velocity = direction * fleeSpeed;
        UpdateJellyfishDirection(direction);
    }

    IEnumerator StingPlayer()
    {
        currentState = State.Stinging;
        isStinging = true;
        rb.velocity = Vector2.zero; // Berhenti saat menyengat

        // Aktifkan animasi sting
        animator.SetTrigger("Sting");
        Debug.Log("Ubur-ubur menyengat pemain!");

        // Efek stun pemain
        if (player.TryGetComponent<PlayerController2>(out PlayerController2 playerController))
        {
            playerController.Stun(stingDuration); // Panggil fungsi stun
        }

        yield return new WaitForSeconds(stingDuration);

        Debug.Log("Sting selesai, ubur-ubur akan menjauh.");
        yield return new WaitForSeconds(fleeAfterStingTime);

        isStinging = false;
        currentState = State.Fleeing;
    }


    void UpdateJellyfishDirection(Vector2 movementDirection)
    {
        Vector3 currentScale = transform.localScale;

        if (movementDirection.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else if (movementDirection.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius); // Radius mengejar

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stingRadius); // Radius menyengat
    }
}
