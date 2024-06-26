using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public int initialScore = 500;
    private int score;
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject VictoryPanel;  // Reference to the VictoryPanel
    public TMPro.TextMeshProUGUI FinalScoreText;      // Reference to display final score
    public TMPro.TextMeshProUGUI LeaderboardText;
    private float timeCounter = 0;

    public void Start()
    {
        score = initialScore;
        UpdatescoreText();
        VictoryPanel.SetActive(false);
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
            UpdatescoreText();
        }
    }

    public void AddScore(int amount)
    {
        if (!isLevelCompleted)
        {
        score += amount;
        UpdatescoreText();
        }
    }

    private void UpdatescoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public int GetScore() {
        return this.score;
    }
    private void AddToLeaderboard(string playerName, int playerScore)
    {
        leaderboard.Add(new LeaderboardEntry { Name = playerName, Score = playerScore });
        leaderboard.Sort((entry1, entry2) => entry2.Score.CompareTo(entry1.Score));  // Sort in descending
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