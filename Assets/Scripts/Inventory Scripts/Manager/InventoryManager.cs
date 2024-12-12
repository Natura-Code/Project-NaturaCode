using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Item[] starItem;
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public InventorySlot slotStore; // Slot khusus untuk store
    public Button sellButton; // Tombol sell

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

        // Inisialisasi tombol sell
        sellButton.gameObject.SetActive(false);
        sellButton.onClick.AddListener(SellSelectedItem);
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
            if (isNumber && number > 0 && number <= inventorySlots.Length)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        // Seleksi otomatis untuk slot store jika ada item
        AutoSelectStoreSlot();
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        // Cek apakah slot terpilih adalah slot store dan memiliki item
        UpdateSellButton();
    }

    void AutoSelectStoreSlot()
    {
        InventoryItem itemInSlot = slotStore.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && selectedSlot != -2) // -2 untuk menandai slotStore
        {
            if (selectedSlot >= 0) inventorySlots[selectedSlot].Deselect();
            slotStore.Select();
            selectedSlot = -2; // Menandai bahwa slotStore sedang dipilih

            UpdateSellButton();
        }
    }

    void UpdateSellButton()
    {
        InventoryItem itemInSlot = null;

        if (selectedSlot == -2) // Jika slotStore terpilih
        {
            itemInSlot = slotStore.GetComponentInChildren<InventoryItem>();
        }
        else if (selectedSlot >= 0)
        {
            itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        }

        sellButton.gameObject.SetActive(itemInSlot != null);
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
        InventorySlot slot = selectedSlot == -2 ? slotStore : inventorySlots[selectedSlot];
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

    void SellSelectedItem()
    {
        // Fokus hanya pada slotStore
        InventoryItem itemInSlot = slotStore.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            Debug.Log("Item sold: " + itemInSlot.item.name);

            // Tambahkan logika penjualan (misalnya menambahkan poin atau currency)
            FindObjectOfType<PlayerWallet>().AddMoney(itemInSlot.item.sellValue);


            // Kurangi jumlah item atau hapus jika sudah habis
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
            }
            else
            {
                itemInSlot.RefreshCount();
            }

            // Perbarui tombol "Sell"
            UpdateSellButton();
        }
        else
        {
            Debug.Log("No item to sell in slot store.");
        }
    }


    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count > 0)
            {
                itemInSlot.count--;
                itemInSlot.RefreshCount();

                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }

                return true;
            }
        }

        // Periksa slot store
        InventoryItem storeItem = slotStore.GetComponentInChildren<InventoryItem>();
        if (storeItem != null && storeItem.item == item && storeItem.count > 0)
        {
            storeItem.count--;
            storeItem.RefreshCount();

            if (storeItem.count <= 0)
            {
                Destroy(storeItem.gameObject);
            }

            return true;
        }

        return false;
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

        // Simpan slot store
        InventoryItem storeItem = slotStore.GetComponentInChildren<InventoryItem>();
        if (storeItem != null)
        {
            inventoryData.Add(new InventoryData
            {
                itemName = storeItem.item.name,
                count = storeItem.count
            });
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