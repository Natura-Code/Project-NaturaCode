using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public ShopManager shopManager;

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (shopManager == null)
        {
            Debug.LogError("ShopManager is not assigned!");
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
