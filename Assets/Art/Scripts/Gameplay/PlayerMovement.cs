using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // Komponen CharacterController dari Unity
    public float speed = 6f; // Kecepatan bergerak
    public float gravity = -9.81f; // Gaya gravitasi
    public Transform groundCheck; // Posisi untuk memeriksa apakah player berada di tanah
    public float groundDistance = 0.4f; // Jarak untuk ground check
    public LayerMask groundMask; // Layer tanah
    public float jumpHeight = 2f; // Ketinggian lompatan

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        // Mengecek apakah player sedang di tanah
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset gaya gravitasi saat di tanah
        }

        // Mendapatkan input gerakan dari keyboard
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Menggerakkan player
        controller.Move(move * speed * Time.deltaTime);

        // Lompatan
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Menerapkan gravitasi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
