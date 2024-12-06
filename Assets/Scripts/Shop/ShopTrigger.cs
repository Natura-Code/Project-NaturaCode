using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public ShopManager shopManager;

    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered shop area. Press 'E' to open shop.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerInRange = false;
            shopManager.ToggleShop(false);
            Debug.Log("Player left shop area.");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            shopManager.ToggleShop(true);
        }
    }
}


