using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    private bool isStoreOpen = false;
    [SerializeField] private GameObject settingMenuPanel;
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private GameObject storePanel;
    //[SerializeField] private GameObject inventoryPanel;

    void Update()
    {
        if (settingMenuPanel.activeSelf) //|| inventoryPanel.activeSelf
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isStoreOpen)
            {
                BackButtonStore();
            }
            else
            {
                StoreButton();
            }
        }
    }
    public void StoreButton()
    {
        gameUIPanel.SetActive(false);
        storePanel.SetActive(true);
        isStoreOpen = true;
        Time.timeScale = 0f;
    }

    public void BackButtonStore()
    {
        storePanel.SetActive(false);
        gameUIPanel.SetActive(true);
        isStoreOpen = false;
        Time.timeScale = 1f;
    }
}
