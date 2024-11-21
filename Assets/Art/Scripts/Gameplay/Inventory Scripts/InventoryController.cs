using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
    [SerializeField] private GameObject settingMenuPanel;
    [SerializeField] private GameObject pilihanMenuPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject storePanel;
    private int inventorySize = 42;

    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
    }

    public void Update()
    {
        if (storePanel.activeSelf || pilihanMenuPanel.activeSelf)
        {
            return; 
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                InventoryButton();
            }
            else
            {
                inventoryUI.Hide();
                BackButtonInventory();
            }

        }
    }
    public void InventoryButton()
    {
        settingMenuPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BackButtonInventory()
    {
        inventoryPanel.SetActive(false);
        settingMenuPanel.SetActive(true);
        Time.timeScale = 1f;
    }
}

