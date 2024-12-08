using UnityEngine;

public class VolumePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel; // Panel utama UI game
    [SerializeField] private GameObject volumePanel; // Panel pengaturan volume

    private bool isVolumePanelOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isVolumePanelOpen)
            {
                CloseVolumePanelWithKey();
            }
            else
            {
                OpenVolumePanelWithKey();
            }
        }
    }

    public void OpenVolumePanelWithKey()
    {
        volumePanel.SetActive(true);
        isVolumePanelOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseVolumePanelWithKey()
    {
        volumePanel.SetActive(false);
        isVolumePanelOpen = false;
        Time.timeScale = 1f;
    }
    public void OpenVolumePanel()
    {
        volumePanel.SetActive(true);
        settingPanel.SetActive(false);
        isVolumePanelOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseVolumePanel()
    {
        settingPanel.SetActive(true);
        volumePanel.SetActive(false);
        isVolumePanelOpen = false;
        Time.timeScale = 1f;
    }
}
