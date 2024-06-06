using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Gameplay;
using TMPro;

namespace Platformer.UI
{
    /// <summary>
    /// A simple controller for switching between UI panels.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        public GameObject[] panels;
        public TextMeshProUGUI highScoreText1;
        public TextMeshProUGUI highScoreText2;

        public void SetActivePanel(int index)
        {
            if (index == 0) DisplayHighScores();
            for (var i = 0; i < panels.Length; i++)
            {
                var active = i == index;
                var g = panels[i];
                if (g.activeSelf != active) g.SetActive(active);
            }
        }

        void OnEnable()
        {
            SetActivePanel(0);
        }

        public void DisplayHighScores()
        {
            var highScoreManager = GameObject.FindObjectOfType<HighScoreManager>();

            highScoreText1.text = "";
            highScoreText2.text = "";
            for (int i = 0; i < highScoreManager.highScoreList.highScores.Count; i++)
            {
                string entryText = $"{i + 1}. " + $"{highScoreManager.highScoreList.highScores[i].playerName}: {highScoreManager.highScoreList.highScores[i].score}\n";
                if (i < 5)
                {
                    highScoreText1.text += entryText;
                }
                else
                {
                    highScoreText2.text += entryText;
                }
            }

        }

        public void exitGame() {
            SceneManager.LoadScene("StartScene");
        }
    }
}