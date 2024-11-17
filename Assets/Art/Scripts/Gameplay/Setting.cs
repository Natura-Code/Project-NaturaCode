using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject settingMenuPanel;
    [SerializeField] private GameObject pilihanMenuPanel;
    [SerializeField] private GameObject storePanel;
    [SerializeField] private GameObject inventoryPanel;

    void Start()
    {
        settingMenuPanel.SetActive(true);
        pilihanMenuPanel.SetActive(false);
    }

    void Update()
    {

    }

    public void InGameButton(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void InGame2Button(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void PilihanButton()
    {
        settingMenuPanel.SetActive(false);
        pilihanMenuPanel.SetActive(true);
    }

    public void BackButtonPilihanMenu()
    {
        settingMenuPanel.SetActive(true);
        pilihanMenuPanel.SetActive(false);
    }

    public void StoreButton()
    {
        pilihanMenuPanel.SetActive(false);
        storePanel.SetActive(true);
    }

    public void BackButtonStore()
    {
        pilihanMenuPanel.SetActive(true);
        storePanel.SetActive(false);
    }

    public void InventoryButton()
    {
        pilihanMenuPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public void BackButtonInventory()
    {
        pilihanMenuPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }
}
