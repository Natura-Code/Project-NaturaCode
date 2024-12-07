using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Item[] starItem; 
    public int maxStackedItems = 4; 
    public InventorySlot[] inventorySlots; 
    public GameObject inventoryItemPrefab; 

    int selectedSlot = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
        if (!LoadInventory()) 
        {
            foreach (var item in starItem)
            {
                AddItem(item);
            }
        }
    }

    private void OnDisable()
    {
        SaveInventory(); 
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 6)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }
        return null;
    }

    void SaveInventory()
    {
        List<InventoryData> inventoryData = new List<InventoryData>();
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                inventoryData.Add(new InventoryData
                {
                    itemName = itemInSlot.item.name,
                    count = itemInSlot.count
                });
            }
        }

        string json = JsonUtility.ToJson(new InventoryWrapper { inventoryList = inventoryData });
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
        Debug.Log("Inventory saved: " + json);
    }

    bool LoadInventory()
    {
        string json = PlayerPrefs.GetString("InventoryData", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            InventoryWrapper wrapper = JsonUtility.FromJson<InventoryWrapper>(json);
            foreach (var data in wrapper.inventoryList)
            {
                Item item = System.Array.Find(starItem, i => i.name == data.itemName);
                for (int i = 0; i < data.count; i++)
                {
                    AddItem(item);
                }
            }
            Debug.Log("Inventory loaded: " + json);
            return true; 
        }

        return false; 
    }

}

[System.Serializable]
public class InventoryWrapper
{
    public List<InventoryData> inventoryList;
}

[System.Serializable]
public class InventoryData
{
    public string itemName;
    public int count;
}
