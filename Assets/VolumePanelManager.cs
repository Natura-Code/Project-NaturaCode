using UnityEngine;

public class VolumePanelManager : MonoBehaviour
{
    //[SerializeField] private GameObject gameUIPanel; // Panel utama UI game
    [SerializeField] private GameObject volumePanel; // Panel pengaturan volume

    private bool isVolumePanelOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isVolumePanelOpen)
            {
                CloseVolumePanel();
            }
            else
            {
                OpenVolumePanel();
            }
        }
    }

    public void OpenVolumePanel()
    {
        volumePanel.SetActive(true);
        //gameUIPanel.SetActive(false);
        isVolumePanelOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseVolumePanel()
    {
        //gameUIPanel.SetActive(true);
        volumePanel.SetActive(false);
        isVolumePanelOpen = false;
        Time.timeScale = 1f;
    }
}
