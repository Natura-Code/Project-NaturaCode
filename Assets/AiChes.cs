using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AiChes : MonoBehaviour
{
    [Header("AI Settings")]
    public float speed = 5f; // Kecepatan ubur-ubur
    public float distanceBetween = 5f; // Radius untuk mengejar pemain
    public string playerTag = "Player"; // Tag untuk mengenali pemain
    public float stunDuration = 2f; // Durasi stun pemain
    public float fleeDistance = 3f; // Jarak untuk ubur-ubur menjauh setelah menyengat

    private Transform playerTransform; // Transform pemain
    private Transform aiTransform; // Transform ubur-ubur
    private bool isFleeing = false; // Status apakah ubur-ubur sedang menjauh

    void Start()
    {
        // Temukan pemain berdasarkan tag
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            playerTransform = player.transform;
        }

        aiTransform = transform;
    }

    void Update()
    {
        // Jika pemain belum ditemukan, hentikan eksekusi
        if (playerTransform == null) return;

        // Hitung jarak ke pemain
        float distance = Vector2.Distance(aiTransform.position, playerTransform.position);

        if (isFleeing)
        {
            // Jika ubur-ubur sedang menjauh, gerakkan menjauh dari pemain
            FleeFromPlayer();
        }
        else if (distance < distanceBetween)
        {
            // Jika dalam radius, kejar pemain
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - aiTransform.position).normalized;

        // Kejar pemain
        aiTransform.position = Vector2.MoveTowards(
            aiTransform.position,
            playerTransform.position,
            speed * Time.deltaTime
        );
    }

    private void FleeFromPlayer()
    {
        Vector2 fleeDirection = (aiTransform.position - playerTransform.position).normalized;

        // Menjauh dari pemain
        aiTransform.position = Vector2.MoveTowards(
            aiTransform.position,
            aiTransform.position + (Vector3)fleeDirection,
            speed * Time.deltaTime
        );

        // Berhenti menjauh jika sudah cukup jauh
        if (Vector2.Distance(aiTransform.position, playerTransform.position) > fleeDistance)
        {
            isFleeing = false; // Kembali ke normal
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Periksa apakah yang disentuh adalah pemain
        if (other.CompareTag(playerTag) && !isFleeing)
        {
            Debug.Log("Ubur-ubur menyengat pemain!");

            // Beri stun ke pemain
            PlayerController2 playerController = other.GetComponent<PlayerController2>();
            if (playerController != null)
            {
                playerController.Stun(stunDuration);
            }

            // Mulai proses menjauh
            isFleeing = true;
        }
    }
}