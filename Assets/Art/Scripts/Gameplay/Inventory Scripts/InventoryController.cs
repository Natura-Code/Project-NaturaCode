using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
     private int inventorySize = 42;

    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }
    
}

