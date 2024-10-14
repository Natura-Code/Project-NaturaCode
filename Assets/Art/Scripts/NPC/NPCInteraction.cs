using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Dialogue dialogue; // Referensi ke asset dialog untuk NPC

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah objek yang berinteraksi adalah pemain
        if (collision.CompareTag("Player"))
        {
            // Mengaktifkan interaksi dengan NPC
            Actor actor = collision.GetComponent<Actor>();
            if (actor != null)
            {
                actor.Dialogue = dialogue; // Set dialog NPC
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Cek jika pemain keluar dari area trigger
        if (collision.CompareTag("Player"))
        {
            Actor actor = collision.GetComponent<Actor>();
            if (actor != null)
            {
                actor.Dialogue = null; // Reset dialog
            }
        }
    }
}

