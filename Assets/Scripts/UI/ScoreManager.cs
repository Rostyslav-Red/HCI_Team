using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

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

    // Simple leaderboard
    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    // Changed Start to public Initialize to explicitly call it if needed from other scripts
    public void Initialize()
    {
        score = initialScore;
        UpdateScoreText();
        VictoryPanel.SetActive(false);  // Hide victory panel initially
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
                CompleteLevel();
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

    public void CompleteLevel()
    {
        isLevelCompleted = true;
        Time.timeScale = 0f;  // Pause the game
        VictoryPanel.SetActive(true);  // Show victory panel
        FinalScoreText.text = "Final Score: " + score;  // Display the final score
        AddToLeaderboard("Player One", score);  // Add the player to the leaderboard
        UpdateLeaderboardDisplay();
        Debug.Log("Level completed! Final score: " + score);
    }

    private void AddToLeaderboard(string playerName, int playerScore)
    {
        leaderboard.Add(new LeaderboardEntry { Name = playerName, Score = playerScore });
        leaderboard.Sort((entry1, entry2) => entry2.Score.CompareTo(entry1.Score));  // Sort in descending order
    }

    private void UpdateLeaderboardDisplay()
    {
        LeaderboardText.text = "Leaderboard:\n";
        foreach (var entry in leaderboard)
        {
            LeaderboardText.text += entry.Name + " - " + entry.Score + "\n";
        }
    }

    private class LeaderboardEntry
    {
        public string Name;
        public int Score;
    }
}
