using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int initialScore = 500;
    private int score;
    public TMPro.TextMeshProUGUI scoreText;
    private float timeCounter = 0;

    public void Start()
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
}