using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // Dialog yang akan dimulai
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>(); // Mengambil reference ke DialogueManager
    }

    // Fungsi untuk memulai dialog
    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue); // Memulai dialog
    }
}
