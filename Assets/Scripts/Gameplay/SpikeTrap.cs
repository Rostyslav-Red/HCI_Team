using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

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
                    health.Decrement();
                }
            }
        }
    }
}
