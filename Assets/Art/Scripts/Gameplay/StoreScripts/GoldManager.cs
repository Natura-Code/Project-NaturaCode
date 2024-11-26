using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI goldTextUI;
    [SerializeField] private TextMeshProUGUI warningText;

    private int gold = 0;
    private static GoldManager instance;

    void Start()
    {
        gold = PlayerPrefs.GetInt("Gold", 0);
        UpdateGoldText();
        warningText.text = "";
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
            PlayerPrefs.SetInt("Gold", gold);
            UpdateGoldText();
            warningText.text = "";
        }
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            PlayerPrefs.SetInt("Gold", gold);
            UpdateGoldText();
            return true; 
        }
        return false; 
    }

    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + gold.ToString();
        goldTextUI.text = "Gold: " + gold.ToString();
    }

    private IEnumerator ShowWarningTemporary(string message, float delay)
    {
        warningText.text = message;
        yield return new WaitForSecondsRealtime(delay);
        warningText.text = "";
    }

    public void SetGoldTextReferences(TextMeshProUGUI newGoldText, TextMeshProUGUI newGoldTextUI, TextMeshProUGUI newWarningText)
    {
        goldText = newGoldText;
        goldTextUI = newGoldTextUI;
        warningText = newWarningText;
        UpdateGoldText();
    }
    public int GetGold()
    {
        return PlayerPrefs.GetInt("Gold", 0); 
    }

}
