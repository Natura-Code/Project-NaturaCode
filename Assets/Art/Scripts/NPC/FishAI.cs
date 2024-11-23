using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    [Header("Player Interaction")]
    public Transform player; // Objek pemain
    public float fleeRadius = 3f; // Jarak ikan mulai menjauh
    public float fleeSpeed = 4f; // Kecepatan saat lari menjauh

    [Header("Normal Behavior")]
    public float normalSpeed = 2f; // Kecepatan normal
    public float directionChangeInterval = 2f; // Interval pergantian arah acak

    [Header("Boundary Settings")]
    public BoxCollider2D boundaryCollider; // Collider pembatas

    private Vector2 targetDirection; // Arah gerak acak
    private Rigidbody2D rb; // Untuk menggerakkan objek dengan Rigidbody
    private Animator animator; // Referensi Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Coba cari objek player secara otomatis jika belum diatur
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Player tidak ditemukan! Pastikan ada objek dengan tag 'Player' di scene.");
            }
        }

        // Mulai rutinitas perubahan arah
        StartCoroutine(ChangeDirectionRoutine());
    }

    void FixedUpdate()
    {
        if (player != null) // Pastikan player tidak null
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < fleeRadius)
            {
                // Aktifkan animasi flee
                animator.SetBool("isFleeing", true);
                FleeFromPlayer();
            }
            else
            {
                // Matikan animasi flee (kembali ke idle)
                animator.SetBool("isFleeing", false);
                MoveInRandomDirection();
            }
        }
        else
        {
            Debug.LogWarning("Player masih belum diatur, ikan hanya akan bergerak secara normal.");
            MoveInRandomDirection();
        }

        // Batasi pergerakan ikan dalam boundary
        RestrictMovementWithinBoundary();
    }

    void FleeFromPlayer()
    {
        if (player != null) // Pastikan player tidak null
        {
            Vector2 fleeDirection = ((Vector2)transform.position - (Vector2)player.position).normalized;
            rb.velocity = fleeDirection * fleeSpeed;

            // Perbarui arah ikan berdasarkan arah flee
            UpdateFishDirection(fleeDirection);
        }
    }

    void MoveInRandomDirection()
    {
        rb.velocity = targetDirection * normalSpeed;

        // Perbarui arah ikan berdasarkan gerakan acak
        UpdateFishDirection(targetDirection);
    }

    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            PickRandomDirection();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void PickRandomDirection()
    {
        targetDirection = Random.insideUnitCircle.normalized;
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
            Debug.LogWarning("Boundary Collider belum diatur! Ikan tidak akan dibatasi pergerakannya.");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualisasi radius menjauh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);

        // Visualisasi boundary 
        if (boundaryCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundaryCollider.bounds.center, boundaryCollider.bounds.size);
        }
    }

    void UpdateFishDirection(Vector2 movementDirection)
    {
        Vector3 currentScale = transform.localScale; // Ambil skala saat ini

        if (movementDirection.x > 0)
        {
            // Menghadap kanan (pastikan X positif)
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else if (movementDirection.x < 0)
        {
            // Menghadap kiri (pastikan X negatif)
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
    }
}
