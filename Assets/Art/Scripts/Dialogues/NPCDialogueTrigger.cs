using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // Dialog NPC
    private DialogueManager dialogueManager;

    void Start()
    {
        // Mencari komponen DialogueManager di scene
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Fungsi untuk mendeteksi player berada di dalam area trigger
    void OnTriggerStay2D(Collider2D other)
    {
        // Periksa apakah yang memasuki area trigger adalah player dan menekan tombol E
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue(); // Panggil fungsi untuk memulai dialog
        }
    }

    // Memulai dialog menggunakan DialogueManager
    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue); // Mulai dialog saat player menekan tombol interaksi
    }
}
