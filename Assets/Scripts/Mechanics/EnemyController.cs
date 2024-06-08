using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;  // Reuse this component for the death sound

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;
        Animator animator;  // Add a reference to the Animator

        public Bounds Bounds => _collider.bounds;
        private bool isDead = false;  // Add a flag to indicate if the enemy is dead

        private Vector3 previousPosition;  // Track previous position for direction

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();  // Get the Animator component
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        void Update()
        {
            if (isDead) return;  // If the enemy is dead, do not update its position

            if (path != null)
            {
                if (mover == null)
                    mover = path.CreateMover(control.maxSpeed * 0.5f);

                Vector3 targetPosition = new Vector3(mover.Position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, control.maxSpeed * Time.deltaTime);

                // Flip the sprite based on movement direction
                if (transform.position.x > previousPosition.x)
                {
                    spriteRenderer.flipX = false;  // Facing right
                }
                else if (transform.position.x < previousPosition.x)
                {
                    spriteRenderer.flipX = true;  // Facing left
                }

                previousPosition = transform.position;  // Update previous position
            }
        }

        public void Die()
        {
            isDead = true;
            if (animator != null)
            {
                animator.SetTrigger("death");  // Trigger the death animation
                Debug.Log("Death animation triggered.");
            }
            if (_audio != null && ouch != null)
            {
                _audio.PlayOneShot(ouch);  // Play the death sound
            }
            _collider.enabled = false;  // Disable the collider to prevent further interactions
            StartCoroutine(RemoveAfterDeath());  // Start a coroutine to remove the enemy after the animation
        }

        private IEnumerator RemoveAfterDeath()
        {
            float waitTime = 0.85f;  // Adjust based on actual animation length
            Debug.Log("Waiting for " + waitTime + " seconds before removing enemy.");
            yield return new WaitForSeconds(waitTime);  // Wait for the animation to play out
            Debug.Log("Removing enemy from scene.");
            Destroy(gameObject);  // Remove the enemy from the scene
        }
    }
}
