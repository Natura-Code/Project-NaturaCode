using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    public GameObject shopUI;
    public Transform container;
    public GameObject shopItemTemplate;

    [Header("Shop Items")]
    public List<Item> shopItems; // Daftar item yang tersedia di toko
    public List<int> shopItemPrices; // Harga masing-masing item

    [Header("References")]
    public DemoScript demoScript; // Referensi ke DemoScript untuk inventory
    public PlayerWallet playerWallet; // Referensi ke dompet pemain

    private void Start()
    {
        shopUI.SetActive(false); // Awalnya shop UI disembunyikan
        PopulateShop();
    }

    private void PopulateShop()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            Item item = shopItems[i];
            int price = shopItemPrices[i];

            // Membuat instansiasi untuk setiap item di toko
            GameObject shopItem = Instantiate(shopItemTemplate, container);
            shopItem.SetActive(true);

            // Mengatur informasi item pada template
            shopItem.transform.Find("nameText").GetComponent<TextMeshProUGUI>().text = item.name;
            shopItem.transform.Find("itemImage").GetComponent<Image>().sprite = item.image;
            shopItem.transform.Find("priceText").GetComponent<TextMeshProUGUI>().text = "Price: " + price;

            // Menambahkan listener untuk tombol pembelian
            Button buyButton = shopItem.GetComponent<Button>();
            buyButton.onClick.AddListener(() => AttemptPurchase(item, price));
        }
    }

    private void AttemptPurchase(Item item, int price)
    {
        // Cek apakah pemain memiliki cukup uang
        if (playerWallet.SpendMoney(price))
        {
            bool result = demoScript.inventoryManager.AddItem(item); // Tambahkan item ke inventory

            if (result)
            {
                Debug.Log("Purchased item: " + item.name);
            }
            else
            {
                Debug.Log("Inventory full! Refund money.");
                playerWallet.AddMoney(price); // Kembalikan uang jika inventory penuh
            }
        }
        else
        {
            Debug.Log("Cannot purchase. Not enough money!");
        }
    }

    public void ToggleShop(bool show)
    {
        shopUI.SetActive(show);
    }
}
