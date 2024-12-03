using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);

        if (result == true)
        {
            Debug.Log("Item added!.");
        }
        else
        {
            Debug.Log("Item not added!.");
        }
    }

    public void GetSelectedItem(bool use)
    {
        Item recivedItem = inventoryManager.GetSelectedItem(false);
        if (recivedItem != null)
        {
            Debug.Log("Recived item: " + recivedItem);
        }
        else
        {
            Debug.Log("No item recived.");
        }
    }
    public void UseSelectedItem(bool use)
    {
        Item recivedItem = inventoryManager.GetSelectedItem(true);
        if (recivedItem != null)
        {
            Debug.Log("Used item: " + recivedItem);
        }
        else
        {
            Debug.Log("No item used.");
        }
    }
}
