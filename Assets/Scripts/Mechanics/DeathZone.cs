using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// DeathZone components mark a collider which will schedule a
    /// PlayerEnteredDeathZone event when the player enters the trigger.
    /// </summary>
    public class DeathZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Schedule<PlayerEnteredDeathZone>();
                ev.deathzone = this;
            }
        }

        void OnDrawGizmos()
        {
            // Set the color of the Gizmos
            Gizmos.color = Color.red;
            // Draw a wireframe cube with the same position and size as the BoxCollider2D
            var collider = GetComponent<BoxCollider2D>();
            Gizmos.DrawWireCube(transform.position + (Vector3)collider.offset, collider.size);
        }


    }
}