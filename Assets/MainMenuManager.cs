using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        PlayerPrefs.SetFloat("InGame_PosX", 0f);
        PlayerPrefs.SetFloat("InGame_PosY", 0f);

        PlayerPrefs.SetFloat("InGameSea_X", 0f);
        PlayerPrefs.SetFloat("InGameSea_Y", 0f);

        PlayerPrefs.SetInt("Gold", 0);

        PlayerPrefs.Save();

        Debug.Log("New game data created and saved.");

        SceneManager.LoadScene("InGame"); 
    }

    public void Play()
    {
        if (PlayerPrefs.HasKey("InGame_PosX") || PlayerPrefs.HasKey("Gold"))
        {
            Debug.Log("Loading saved game...");
            SceneManager.LoadScene("InGame"); 
        }
        else
        {
            Debug.LogWarning("Save tidak dtemukan! Muat Game baru.");
            NewGame();
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }
}
