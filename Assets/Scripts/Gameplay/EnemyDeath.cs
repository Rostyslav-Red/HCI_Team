using Platformer.Core;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when an enemy dies.
    /// </summary>
    public class EnemyDeath : Simulation.Event<EnemyDeath>
    {
        public EnemyController enemy;

        public override void Execute()
        {
            if (enemy != null)
            {
                enemy.Die();  // Call the Die method on the enemy
            }
        }
    }
}
