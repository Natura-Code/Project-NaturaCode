using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Image LoadingBarFill;

    public void LoadScene(string scenename)
    {
        StartCoroutine(LoadSceneAsync(scenename));
    }

    IEnumerator LoadSceneAsync(string scenename)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenename);
        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            
            LoadingBarFill.fillAmount = progressValue;
            
            yield return null;
        }
    }
}
