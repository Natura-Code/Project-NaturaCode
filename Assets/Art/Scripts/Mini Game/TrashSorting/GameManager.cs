using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text scoreText;
    public Text timerText;

    private int score = 0;
    private float timeRemaining = 60f; // 60 seconds

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
        }
        else
        {
            EndGame();
        }
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score.ToString();
    }

    private void EndGame()
    {
        Debug.Log("Game Over! Final Score: " + score);
        // Display end-game UI or restart logic
    }
}

