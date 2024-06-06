using Platformer.UI;
using UnityEngine;

namespace Platformer.Gameplay {
    public class HighScoreManager : MonoBehaviour
    {
        public HighScoreList highScoreList = new HighScoreList();

        void Start()
        {
            LoadHighScores();
        }

        public void CheckAndAddHighScore(int score, string playerName)
        {
            var mainUi = GameObject.FindAnyObjectByType<MainUIController>();
            if (highScoreList.IsHighScore(score))
            {
                highScoreList.AddHighScore(score, playerName);
                SaveHighScores();
            }
        }

        private void SaveHighScores()
        {
            string json = JsonUtility.ToJson(highScoreList);
            PlayerPrefs.SetString("HighScores", json);
            PlayerPrefs.Save();
        }

        private void LoadHighScores()
        {
            if (PlayerPrefs.HasKey("HighScores"))
            {
                string json = PlayerPrefs.GetString("HighScores");
                highScoreList = JsonUtility.FromJson<HighScoreList>(json);
            }
        }
    }
}