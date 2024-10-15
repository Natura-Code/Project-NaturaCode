using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 2f;  // Kecepatan NPC bergerak
    public Transform[] waypoints; // Titik-titik tempat NPC berjalan
    private int waypointIndex = 0;

    private void Update()
    {
        Move();
    }

    void Move()
    {
        // NPC bergerak ke arah waypoint
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, moveSpeed * Time.deltaTime);

        // Jika sudah mencapai waypoint, pindah ke waypoint berikutnya
        if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < 0.2f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0; // Kembali ke waypoint pertama setelah menyelesaikan rute
            }
        }
    }
}
