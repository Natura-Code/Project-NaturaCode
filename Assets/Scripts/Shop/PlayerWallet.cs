using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public int currentMoney = 0; 

    private const string MoneyKey = "PlayerMoney";

    void Start()
    {
        currentMoney = PlayerPrefs.GetInt(MoneyKey, currentMoney);
        Debug.Log("Loaded money: " + currentMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            SaveMoney(); 
            Debug.Log("Money spent: " + amount + ". Current money: " + currentMoney);
            return true;
        }
        else
        {
            Debug.Log("Not enough money! Current money: " + currentMoney);
            return false;
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        SaveMoney(); 
        Debug.Log("Money added: " + amount + ". Current money: " + currentMoney);
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(MoneyKey, currentMoney); 
        PlayerPrefs.Save();
        Debug.Log("Money saved: " + currentMoney);
    }

    void OnApplicationQuit()
    {
        SaveMoney(); 
    }
}
