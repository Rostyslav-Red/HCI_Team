using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;

public class ScoreManager : MonoBehaviour
{
    public int initialScore = 500;
    private int score;
    public TextMeshProUGUI scoreText;
    public GameObject VictoryPanel;  // Reference to the VictoryPanel
    public TextMeshProUGUI FinalScoreText;      // Reference to display final score
    public TextMeshProUGUI LeaderboardText;
    private float timeCounter = 0;
    public float timeLimit = 60.0f;  // Total time for the level
    private bool isLevelCompleted = false;

    // Changed Start to public Initialize to explicitly call it if needed from other scripts
    public void Initialize()
    {
        score = initialScore;
        UpdateScoreText();
    }

    void Update()
    {
        if (!isLevelCompleted)
        {
            // Update time
            timeCounter += Time.deltaTime;
            if (timeCounter >= 5.0f)
            {
                score -= 50;  // Deduct points every 5 seconds
                timeCounter = 0;  // Reset the counter
                UpdateScoreText();
            }

            // Check if time is up
            if (Time.timeSinceLevelLoad > timeLimit)
            {
                Schedule<PlayerDeath>();
            }
        }
    }

    // Public method to get the current score
    public int GetScore()
    {
        return score;
    }

    public void AddScore(int amount)
    {
        if (!isLevelCompleted)
        {
            score += amount;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private class LeaderboardEntry
    {
        public string Name;
        public int Score;
    }
}
