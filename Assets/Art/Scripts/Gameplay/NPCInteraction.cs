using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager; // Referensi ke DialogueManager
    public Dialogue initialDialogue; // Dialog awal yang akan dimulai

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue(initialDialogue); // Memulai dialog ketika player mendekat
        }
    }
}