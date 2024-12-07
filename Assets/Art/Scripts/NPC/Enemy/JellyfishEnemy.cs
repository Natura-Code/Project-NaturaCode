using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float wanderSpeed = 2f; // Kecepatan gerakan bebas
    public float chaseSpeed = 3.5f; // Kecepatan saat mengejar
    public float fleeSpeed = 4f; // Kecepatan saat menjauh
    public float wanderRadius = 5f; // Radius gerakan bebas
    public float directionChangeInterval = 2f; // Interval ubah arah saat bebas

    [Header("Attack Settings")]
    public float attackRadius = 3f; // Radius untuk menyerang pemain
    public float fleeDistance = 5f; // Jarak minimum untuk menjauh setelah menyerang
    public float stunDuration = 2f; // Durasi stun untuk pemain

    private Vector2 wanderDirection; // Arah gerakan bebas
    private Transform playerTransform; // Transform pemain
    private Rigidbody2D rb; // Rigidbody ubur-ubur
    private bool isFleeing = false; // Status apakah sedang menjauh
    private bool isAttacking = false; // Status apakah sedang menyerang

    private BoxCollider2D boundaryCollider; // Collider batas area

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Cari pemain berdasarkan tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Cari boundary berdasarkan tag
        GameObject boundary = GameObject.FindGameObjectWithTag("Boundary");
        if (boundary != null)
        {
            boundaryCollider = boundary.GetComponent<BoxCollider2D>();
            if (boundaryCollider == null)
            {
                Debug.LogWarning("GameObject dengan tag 'Boundary' tidak memiliki BoxCollider2D!");
            }
        }
        else
        {
            Debug.LogWarning("Boundary dengan tag 'Boundary' tidak ditemukan!");
        }

        // Atur gerakan bebas pertama
        StartCoroutine(ChangeWanderDirection());
    }

    private void Update()
    {
        if (playerTransform == null || isFleeing || isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < attackRadius)
        {
            // Mulai menyerang jika dalam radius serangan
            StartCoroutine(AttackPlayer());
        }
        else if (distanceToPlayer < wanderRadius)
        {
            // Kejar pemain jika dalam radius kejar
            ChasePlayer();
        }
        else
        {
            // Gerakan bebas seperti ikan
            Wander();
        }
        RestrictMovementWithinBoundary();
    }

    private void Wander()
    {
        rb.velocity = wanderDirection * wanderSpeed;

        // Rotasi untuk orientasi gerakan
        float angle = Mathf.Atan2(wanderDirection.y, wanderDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator ChangeWanderDirection()
    {
        while (!isFleeing && !isAttacking)
        {
            // Ubah arah secara acak
            wanderDirection = Random.insideUnitCircle.normalized;
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;

        // Rotasi untuk mengarah ke pemain
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;

        // Serang pemain
        Debug.Log("Jellyfish attacking the player!");
        PlayerController2 playerController = playerTransform.GetComponent<PlayerController2>();
        if (playerController != null)
        {
            playerController.Stun(stunDuration);
        }

        // Tunggu sejenak sebelum menjauh
        yield return new WaitForSeconds(stunDuration / 2);

        // Mulai menjauh
        StartFleeing();
    }

    private void StartFleeing()
    {
        isAttacking = false;
        isFleeing = true;

        Vector2 fleeDirection = (transform.position - playerTransform.position).normalized;
        rb.velocity = fleeDirection * fleeSpeed;

        StartCoroutine(StopFleeing());
    }

    private IEnumerator StopFleeing()
    {
        // Tunggu hingga cukup jauh dari pemain
        while (Vector2.Distance(transform.position, playerTransform.position) < fleeDistance)
        {
            yield return null;
        }

        // Kembali ke mode normal
        isFleeing = false;
        rb.velocity = Vector2.zero;
    }

    void RestrictMovementWithinBoundary()
    {
        if (boundaryCollider != null)
        {
            // Dapatkan batas collider
            Bounds bounds = boundaryCollider.bounds;

            // Ambil posisi saat ini
            Vector3 currentPosition = transform.position;

            // Batasi posisi ikan agar tetap dalam bounds
            float clampedX = Mathf.Clamp(currentPosition.x, bounds.min.x, bounds.max.x);
            float clampedY = Mathf.Clamp(currentPosition.y, bounds.min.y, bounds.max.y);

            // Update posisi ikan jika keluar dari bounds
            transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        }
        else
        {
            Debug.Log("Boundary Collider belum diatur! Ikan tidak akan dibatasi pergerakannya.");
        }
    }
}
