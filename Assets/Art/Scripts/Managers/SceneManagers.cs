using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour

{
    public void ChangeScene(string InGameSea)
    {
        SceneManager.LoadScene(InGameSea);
    }
}