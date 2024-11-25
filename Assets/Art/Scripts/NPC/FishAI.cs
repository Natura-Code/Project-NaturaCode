using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    [Header("Player Interaction")]
    public string playerTag = "Player"; // Tag untuk objek pemain
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
    private Transform player; // Transform pemain

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Cari objek pemain berdasarkan tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player dengan tag '" + playerTag + "' tidak ditemukan.");
        }

        StartCoroutine(ChangeDirectionRoutine());
    }

    void FixedUpdate()
    {
        if (player == null) return; // Jika player belum ditemukan, hentikan

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

        // Batasi pergerakan ikan
        RestrictMovementWithinBoundary();
    }

    void FleeFromPlayer()
    {
        Vector2 fleeDirection = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.velocity = fleeDirection * fleeSpeed;

        // Perbarui arah ikan berdasarkan arah flee
        UpdateFishDirection(fleeDirection);
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
            Bounds bounds = boundaryCollider.bounds;
            Vector3 currentPosition = transform.position;

            float clampedX = Mathf.Clamp(currentPosition.x, bounds.min.x, bounds.max.x);
            float clampedY = Mathf.Clamp(currentPosition.y, bounds.min.y, bounds.max.y);

            transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);

        if (boundaryCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundaryCollider.bounds.center, boundaryCollider.bounds.size);
        }
    }

    void UpdateFishDirection(Vector2 movementDirection)
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
}