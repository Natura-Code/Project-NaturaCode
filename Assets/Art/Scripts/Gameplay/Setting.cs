using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Setting : MonoBehaviour
{
    public GameObject settingMenuPanel;
    public GameObject pilihanMenuPanel;
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
}
