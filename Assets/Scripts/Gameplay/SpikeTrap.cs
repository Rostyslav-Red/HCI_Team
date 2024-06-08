using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;
using Platformer.Core;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    public class SpikeTrap : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                var health = player.GetComponent<Health>();
                if (health != null)
                {
                    // Decrement health
                    health.Decrement();

                    // Play hurt animation and sound
                    if (player.audioSource && player.ouchAudio)
                    {
                        player.audioSource.PlayOneShot(player.ouchAudio);
                    }
                    player.animator.SetTrigger("hurt");

                    // If health reaches zero, trigger player death
                    if (!health.IsAlive)
                    {
                        var ev = Schedule<PlayerDeath>();
                        ev.Execute();
                    }
                }
            }
        }
    }
}
