using Platformer.Mechanics;
using Platformer.Gameplay;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    /// <summary>
    /// The MetaGameController is responsible for switching control between the high level
    /// contexts of the application, eg the Main Menu and Gameplay systems.
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        /// <summary>
        /// The main UI object which used for the menu.
        /// </summary>
        public MainUIController mainMenu;

        /// <summary>
        /// A list of canvas objects which are used during gameplay (when the main ui is turned off)
        /// </summary>
        public Canvas[] gamePlayCanvasii;

        /// <summary>
        /// The game controller.
        /// </summary>
        public GameController gameController;

        bool showMainCanvas = false;

        public Canvas victoryCanvas;
        public GameObject backgroundWithAnimator;

        public TextMeshProUGUI FinalScoreText;      
        public TextMeshProUGUI LeaderboardText;
        public TextMeshProUGUI LeaderboardText2;

        void OnEnable()
        {
            _ToggleMainMenu(showMainCanvas);
            victoryCanvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Turn the main menu on or off.
        /// </summary>
        /// <param name="show"></param>
        public void ToggleMainMenu(bool show)
        {
            if (this.showMainCanvas != show)
            {
                _ToggleMainMenu(show);
            }
        }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                Time.timeScale = 0;
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
            }
            this.showMainCanvas = show;
        }

        void Update()
        {
            if (Input.GetButtonDown("Menu"))
            {
                ToggleMainMenu(!showMainCanvas);
            }
        }

        public void OnPlayerVictory()
        {
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            int playerScore = scoreManager != null ? scoreManager.GetScore() : 0;
            HighScoreManager highScoreManager = FindObjectOfType<HighScoreManager>();
            PlayerData playerData = FindObjectOfType<PlayerData>();

            var playerName = playerData != null ? playerData.playerName : "Player One";
            if (highScoreManager != null)
            {
                highScoreManager.CheckAndAddHighScore(playerScore, playerName);
            }

            Time.timeScale = 0;
            foreach (var canvas in gamePlayCanvasii)
            {
                canvas.gameObject.SetActive(false);
            }

            victoryCanvas.gameObject.SetActive(true);

            Animator backgroundAnimator = backgroundWithAnimator.GetComponent<Animator>();
            if (backgroundAnimator != null)
            {
                backgroundAnimator.Rebind(); // Resets the animator to its default state
                backgroundAnimator.Play("Cherry Blossom Animation", -1, 0f); // Play the animation from start
            }



            FinalScoreText.text = "Your Score: " + playerScore;
            for (int i = 0; i < highScoreManager.highScoreList.highScores.Count; i++)
            {
                string entryText = $"{i + 1}. " + $"{highScoreManager.highScoreList.highScores[i].playerName}: {highScoreManager.highScoreList.highScores[i].score}\n";
                if (i < 5)
                {
                    LeaderboardText.text += entryText;
                }
                else
                {
                    LeaderboardText2.text += entryText;
                }
            }
        }

        public void exitGame() {
            SceneManager.LoadScene("StartScene");
        }
    }
}
