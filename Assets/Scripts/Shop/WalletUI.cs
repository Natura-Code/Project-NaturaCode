using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WalletUI : MonoBehaviour
{
    public PlayerWallet playerWallet;
    public TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = "Money: " + playerWallet.currentMoney;
    }
}

