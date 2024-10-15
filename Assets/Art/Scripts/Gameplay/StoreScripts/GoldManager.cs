using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;      
    [SerializeField] private TextMeshProUGUI warningText;   
    private int gold = 0;      

    void Start()
    {
        warningText.text = ""; 
        UpdateGoldText();      
    }

   
    public void ChangeGold(int amount)
    {
   
        if (amount < 0 && gold + amount < 0)
        {
            StartCoroutine(ShowWarningTemporary("Gold tidak cukup!", 2f));  
        }
        else
        {
            gold += amount;          
            UpdateGoldText();        
            warningText.text = "";   
        }
    }

    
    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + gold.ToString();
    }

    
    private IEnumerator ShowWarningTemporary(string message, float delay)
    {
        warningText.text = message;     
        yield return new WaitForSeconds(delay);  
        warningText.text = "";          
    }
}
