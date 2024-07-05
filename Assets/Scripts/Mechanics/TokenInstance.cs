using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;
using System.Collections;



namespace Platformer.Mechanics
{
    /// <summary>
    /// This class contains the data required for implementing token collection mechanics.
    /// It does not perform animation of the token, this is handled in a batch by the 
    /// TokenController in the scene.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
        public AudioClip tokenCollectAudio;
        [Tooltip("If true, animation will start at a random position in the sequence.")]
        public int value = 30;
        public bool randomAnimationStartTime = false;
        [Tooltip("List of frames that make up the animation.")]
        public Sprite[] idleAnimation, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];

        internal SpriteRenderer _renderer;

        //unique index which is assigned by the TokenController in a scene.
        internal int tokenIndex = -1;
        internal TokenController controller;
        //active frame in animation, updated by the controller.
        internal int frame = 0;
        internal bool collected = false;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            sprites = idleAnimation;  // Ensure this is before setting the frame if randomizing
            if (randomAnimationStartTime)
                frame = Random.Range(0, sprites.Length);
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

        void OnPlayerEnter(PlayerController player)
        {
            if (collected) return;

            collected = true;
            sprites = collectedAnimation;
            frame = 0;

            StartCoroutine(DisableAfterAnimation());

            // Schedule the collision event
            var ev = Schedule<PlayerTokenCollision>();
            ev.token = this;
            ev.player = player;

        }


        IEnumerator DisableAfterAnimation()
        {
            if (controller == null)
            {
                Debug.LogError("Controller is not assigned!");
                yield break;
            }

            // Calculate wait time based on the number of frames in the collected animation and the frame rate
            float waitTime = collectedAnimation.Length / controller.frameRate;

            // Reset frame to start collected animation from the first frame
            frame = 0;

            // Animate collected frames before disabling the game object
            for (int i = 0; i < collectedAnimation.Length; i++)
            {
                _renderer.sprite = collectedAnimation[i];
                yield return new WaitForSeconds(1f / controller.frameRate); // Wait for one frame duration before continuing to the next frame
            }

            gameObject.SetActive(false);
        }



        public void ResetToken()
        {
            collected = false;
            sprites = idleAnimation;
            frame = 0;
            gameObject.SetActive(true);
        }
    }
}