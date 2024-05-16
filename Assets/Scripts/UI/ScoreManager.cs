using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int initialScore = 500;
    private int score;
    public TMPro.TextMeshProUGUI scoreText;
    private float timeCounter = 0;
    public float timeLimit = 60.0f;  // Total time for the level

    void Start()
    {
        score = initialScore;
        UpdatescoreText();
    }

    void Update()
    {
        // Update time
        timeCounter += Time.deltaTime;
        if (timeCounter >= 5.0f)
        {
            score -= 10;  // Deduct points every 5 seconds
            timeCounter = 0;  // Reset the counter
            UpdatescoreText();
        }

        // Check if time is up
        if (Time.timeSinceLevelLoad > timeLimit)
        {
            EndLevel();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdatescoreText();
    }

    private void UpdatescoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void EndLevel()
    {
        // Here, handle what happens when the level ends, e.g., show game over screen
        Debug.Log("Time's up! Final score: " + score);
    }
}