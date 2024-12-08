using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private const string MaxOxygenKey = "MaxOxygen"; 
    private const string MoneyKey = "PlayerMoney"; 

    public void NewGame()
    {
        PlayerPrefs.SetFloat("InGame_PosX", 0f);
        PlayerPrefs.SetFloat("InGame_PosY", 0f);

        PlayerPrefs.SetFloat("InGameSea_X", 0f);
        PlayerPrefs.SetFloat("InGameSea_Y", 0f);

        PlayerPrefs.SetInt(MoneyKey, 0);

        PlayerPrefs.SetFloat(MaxOxygenKey, 20f);

        PlayerPrefs.Save();

        Debug.Log("New game started. Money set to 0.");

        SceneManager.LoadScene("InGame");
    }

    public void Play()
    {
        if (PlayerPrefs.HasKey("InGame_PosX") && PlayerPrefs.HasKey(MoneyKey) && PlayerPrefs.HasKey(MaxOxygenKey))
        {
            Debug.Log("Loading saved game...");

            SceneManager.LoadScene("InGame");
        }
        else
        {
            Debug.LogWarning("Save tidak ditemukan! Memulai game baru.");
            NewGame(); 
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }
}
