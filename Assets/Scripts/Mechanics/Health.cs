using System;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public int maxHP = 1;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => currentHP > 0;

        public bool isInvincible = false;
        private float invincibilityTimer = 0f;
        public float invincibilityDuration = 2.0f;

        public int currentHP;

        void Update() {
            if (isInvincible) {
                invincibilityTimer -= Time.deltaTime;
                if (invincibilityTimer <= 0f) {
                    isInvincible = false;
                }
            }
        }

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement()
        {
            if (!isInvincible)
            {
                currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
                if (currentHP == 0) {
                    this.Die();
                }
                else
                {
                    isInvincible = true;
                    invincibilityTimer = invincibilityDuration;
                }
            }

        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            currentHP = 0;
            var ev = Schedule<HealthIsZero>();
            ev.health = this;
        }

        public void Reset() {
            currentHP = maxHP;
        }

        void Awake()
        {
            currentHP = maxHP;
        }
    }
}
