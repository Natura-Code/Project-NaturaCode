using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private GameObject settingMenuPanel;
    [SerializeField] private GameObject storePanel;
    //[SerializeField] private GameObject inventoryPanel;

    private bool isSettingMenuOpen = false;

    void Start()
    {
        gameUIPanel.SetActive(true);
        settingMenuPanel.SetActive(false);
        storePanel.SetActive(false);
        //inventoryPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (storePanel.activeSelf )//|| inventoryPanel.activeSelf
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isSettingMenuOpen)
            {
                BackButtonSetting();
            }
            else
            {
                SettingButton();
            }
        }
    }

    public void InGameLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void SettingButton()
    {
        settingMenuPanel.SetActive(true);
        gameUIPanel.SetActive(false);
        isSettingMenuOpen = true;
        Time.timeScale = 0f;
    }

    public void BackButtonSetting()
    {
        gameUIPanel.SetActive(true);
        settingMenuPanel.SetActive(false);
        isSettingMenuOpen = false;
        Time.timeScale = 1f;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
    }
}
