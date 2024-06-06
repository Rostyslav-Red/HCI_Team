using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;

        private ScoreManager scoreManager; 

        public override void Execute()
        {
            scoreManager = GameObject.FindObjectOfType<ScoreManager>(); 
            var willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;

            if (willHurtEnemy)
            {
                var enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Decrement();
                    if (!enemyHealth.IsAlive)
                    {
                        Schedule<EnemyDeath>().enemy = enemy;
                        player.Bounce(2);
                        scoreManager.AddScore(10);

                    }
                    else
                    {
                        player.Bounce(7);
                    }
                }
                else
                {
                    Schedule<EnemyDeath>().enemy = enemy;
                    player.Bounce(2);
                }
            }
            else {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null && playerHealth.IsAlive) {
                    playerHealth.Decrement(); 
                    player.animator.SetTrigger("hurt");
                    scoreManager.AddScore(-50); // Add score for defeating an enemy

                    if (!playerHealth.IsAlive) {
                        player.animator.ResetTrigger("hurt");
                        Schedule<PlayerDeath>();
                    }
                }
            }
        }
    }
}