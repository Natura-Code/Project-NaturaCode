using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f; // Jarak maksimal untuk interaksi
    public LayerMask interactableLayer; // Layer untuk objek yang bisa diinteraksi
    public TextMeshProUGUI interactText; // UI Text untuk menampilkan pesan interaksi

    private GameObject currentInteractable;

    void Update()
    {
        // Memeriksa apakah ada objek interaksi dalam jarak tertentu
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactableLayer))
        {
            currentInteractable = hit.collider.gameObject;

            // Menampilkan teks "Press E to interact" saat ada objek yang bisa diinteraksi
            interactText.text = "Press E to interact";
            interactText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Memulai interaksi dengan objek tersebut
                Interact();
            }
        }
        else
        {
            currentInteractable = null;
            interactText.gameObject.SetActive(false);
        }
    }

    // Fungsi untuk memulai interaksi
    void Interact()
    {
        if (currentInteractable != null)
        {
            DialogueTrigger dialogueTrigger = currentInteractable.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTrigger.TriggerDialogue(); // Memulai dialog
            }
        }
    }
}
