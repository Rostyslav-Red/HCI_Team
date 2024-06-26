using Platformer.Mechanics;
using Platformer.Gameplay;
using UnityEngine;

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

        void OnEnable() {
            _ToggleMainMenu(showMainCanvas);
            victoryCanvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Turn the main menu on or off.
        /// </summary>
        /// <param name="show"></param>
        public void ToggleMainMenu(bool show) {
            if (this.showMainCanvas != show) {
                _ToggleMainMenu(show);
            }
        }

        void _ToggleMainMenu(bool show) {
            if (show) {
                Time.timeScale = 0;
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
            }
            else {
                Time.timeScale = 1;
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
            }
            this.showMainCanvas = show;
        }

        void Update() {
            if (Input.GetButtonDown("Menu")) {
                ToggleMainMenu(show: !showMainCanvas);
            }
        }

        public void OnPlayerVictory() {
            int playerScore = GameObject.FindAnyObjectByType<ScoreManager>().GetScore();
            var highScoreManager = GameObject.FindAnyObjectByType<HighScoreManager>();
            PlayerData playerData = FindObjectOfType<PlayerData>();

            var playerName = playerData?.playerName ?? "Player One";
            highScoreManager.CheckAndAddHighScore(playerScore, playerName);

            Time.timeScale = 0;
            foreach (var canvas in gamePlayCanvasii) {
                canvas.gameObject.SetActive(false);
            }

            victoryCanvas.gameObject.SetActive(true); 
        }
    }
}
