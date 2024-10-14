using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Kecepatan pergerakan dan lompatan
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    // Komponen untuk kontrol
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Ground Check
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    // Arah pergerakan
    private float horizontalMove;

    void Start()
    {
        // Inisialisasi komponen
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Mengambil input horizontal (A/D atau Panah Kiri/Kanan)
        horizontalMove = Input.GetAxis("Horizontal");

        // Mengubah arah sprite sesuai pergerakan
        if (horizontalMove < 0)
        {
            spriteRenderer.flipX = true; // Balik ke kiri
        }
        else if (horizontalMove > 0)
        {
            spriteRenderer.flipX = false; // Arah kanan
        }

        // Cek jika player berada di tanah
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jika player berada di tanah dan menekan tombol lompat (space bar)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Set parameter animasi
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("isGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        // Mengatur kecepatan horizontal karakter
        rb.velocity = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        // Memberikan gaya lompatan
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
