using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public int currentMoney = 100; // Uang awal pemain

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
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
        Debug.Log("Money added: " + amount + ". Current money: " + currentMoney);
    }
}
