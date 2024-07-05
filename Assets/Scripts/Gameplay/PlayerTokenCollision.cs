using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a player collides with a token.
    /// </summary>
    /// <typeparam name="PlayerCollision"></typeparam>
    public class PlayerTokenCollision : Simulation.Event<PlayerTokenCollision>
    {
        public PlayerController player;
        public TokenInstance token;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {

            // Play token collection sound
            AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);

            // Add score for collecting the token
            var scoreManager = GameObject.FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(30); // Assuming each token has a 'value' property
            }
            else
            {
                Debug.LogError("ScoreManager not found in the scene!");
            }
        }
    }
}