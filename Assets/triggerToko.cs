using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerToko : MonoBehaviour
{
    public GameObject shopUI; // GameObject untuk UI toko yang akan diaktifkan/nonaktifkan
    public GameObject objectToHide; // GameObject yang akan disembunyikan saat toko dibuka
    public GameObject objectToShow; // GameObject yang akan dimunculkan saat toko dibuka

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (shopUI == null)
        {
            Debug.LogError("Shop UI is not assigned!");
        }
        else
        {
            shopUI.SetActive(false); // Pastikan UI toko tidak terlihat di awal
        }

        if (objectToHide == null)
        {
            Debug.LogWarning("Object to hide is not assigned. This functionality will be ignored.");
        }

        if (objectToShow == null)
        {
            Debug.LogWarning("Object to show is not assigned. This functionality will be ignored.");
        }
        else
        {
            objectToShow.SetActive(false); // Pastikan objectToShow tidak terlihat di awal
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered shop area. Press 'E' to open shop.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ToggleShop(false); // Pastikan toko tertutup ketika pemain keluar
            Debug.Log("Player left shop area.");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Toggle UI toko berdasarkan status saat ini
            bool isActive = shopUI.activeSelf;
            ToggleShop(!isActive);
        }
    }

    public void ToggleShop(bool state)
    {
        if (shopUI != null)
        {
            shopUI.SetActive(state);
            Debug.Log("Shop UI " + (state ? "opened" : "closed"));
        }

        if (objectToHide != null)
        {
            objectToHide.SetActive(!state); // Sembunyikan saat toko dibuka
            Debug.Log("Object " + objectToHide.name + (state ? " hidden." : " shown."));
        }

        if (objectToShow != null)
        {
            objectToShow.SetActive(state); // Tampilkan saat toko dibuka
            Debug.Log("Object " + objectToShow.name + (state ? " shown." : " hidden."));
        }
    }
}
