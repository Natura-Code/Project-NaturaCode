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

    private Vector2 targetDirection; // Arah gerak acak
    private Rigidbody2D rb; // Untuk menggerakkan objek dengan Rigidbody
    private Animator animator; // Referensi Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Ambil Animator
        StartCoroutine(ChangeDirectionRoutine());
    }

    void FixedUpdate()
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

    void FleeFromPlayer()
    {
        Vector2 fleeDirection = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.velocity = fleeDirection * fleeSpeed;

        Debug.Log("Ikan sedang lari menjauh dari pemain!");
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

    void OnDrawGizmosSelected()
    {
        // Visualisasi radius menjauh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);
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