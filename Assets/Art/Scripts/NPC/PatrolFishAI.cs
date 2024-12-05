using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFishAI : MonoBehaviour
{
    [Header("Player Interaction")]
    public string playerTag = "Player"; // Tag pemain
    public float detectionRadius = 5f; // Radius deteksi pemain
    public float fleeSpeed = 3f; // Kecepatan menjauh saat pemain mendekat

    [Header("Patrol Behavior")]
    public float patrolSpeed = 2f; // Kecepatan patroli
    public Vector2[] patrolPoints; // Titik-titik patroli
    public float waitTimeAtPoint = 1f; // Waktu berhenti di setiap titik patroli

    [Header("Boundary Settings")]
    public BoxCollider2D boundaryCollider; // Collider pembatas

    private Transform player; // Transform pemain
    private Rigidbody2D rb; // Rigidbody untuk menggerakkan ikan
    private Animator animator; // Animator untuk mengatur animasi
    private int currentPatrolIndex = 0; // Indeks titik patroli saat ini
    private bool isWaiting = false; // Apakah ikan sedang berhenti di titik patroli

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Cari pemain berdasarkan tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player dengan tag '" + playerTag + "' tidak ditemukan.");
        }

        // Jika tidak ada patrol points, tambahkan titik acak dalam boundary
        if (patrolPoints.Length == 0 && boundaryCollider != null)
        {
            patrolPoints = GenerateRandomPatrolPoints(5);
        }
    }

    void FixedUpdate()
    {
        if (player == null) return; // Jika pemain tidak ada, hentikan

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            // Aktifkan animasi flee
            animator.SetBool("isFleeing", true);
            FleeFromPlayer();
        }
        else
        {
            // Matikan animasi flee (kembali ke patroli)
            animator.SetBool("isFleeing", false);

            if (!isWaiting)
                Patrol();
        }

        RestrictMovementWithinBoundary();
    }

    void FleeFromPlayer()
    {
        Vector2 fleeDirection = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.velocity = fleeDirection * fleeSpeed;

        UpdateFishDirection(fleeDirection);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return; // Jika tidak ada patrol points, hentikan

        Vector2 targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;

        rb.velocity = direction * patrolSpeed;

        // Jika ikan mencapai titik patroli
        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }

        UpdateFishDirection(direction);
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        rb.velocity = Vector2.zero; // Hentikan ikan
        yield return new WaitForSeconds(waitTimeAtPoint);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Pindah ke titik patroli berikutnya
        isWaiting = false;
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

    Vector2[] GenerateRandomPatrolPoints(int numberOfPoints)
    {
        Vector2[] points = new Vector2[numberOfPoints];
        Bounds bounds = boundaryCollider.bounds;

        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
        }

        return points;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (boundaryCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundaryCollider.bounds.center, boundaryCollider.bounds.size);
        }

        // Visualisasi titik patroli
        Gizmos.color = Color.blue;
        foreach (Vector2 point in patrolPoints)
        {
            Gizmos.DrawSphere(point, 0.2f);
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
