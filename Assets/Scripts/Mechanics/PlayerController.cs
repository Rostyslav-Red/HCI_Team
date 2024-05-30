using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using TMPro;

namespace Platformer.Mechanics
{
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        public TextMeshProUGUI playerName;

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        public Collider2D collider2d;
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        int jumpCount = 0;
        const int maxJumpCount = 2;

        public float dashSpeed = 10f;
        public float dashDuration = 0.1f;
        public float dashCooldown = 2f;

        private bool isDashing = false;
        private float dashTime;
        private float dashCooldownTime;

        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;
        private ScoreManager scoreManager;


        void Awake()
        {
            health = GetComponent<Health>();
            health.maxHP = 3;
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager == null)
            {
                Debug.LogError("ScoreManager not found in the scene!");
            }

            PlayerData playerData = FindObjectOfType<PlayerData>();
            if (playerData != null && playerData.playerName != null)
            {
                this.playerName.text = playerData.playerName;
            }
            else
            {
                this.playerName.text = "Player One";
            }
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");

                if (Input.GetButtonDown("Jump") && (jumpState == JumpState.Grounded || jumpCount < maxJumpCount))
                {
                    jumpState = JumpState.PrepareToJump;
                    stopJump = false;
                    jumpCount++;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }

                if (Input.GetButtonDown("Dash") && !isDashing && Time.time >= dashCooldownTime) {
                    StartCoroutine(playerDash());
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    jumpCount = 0; // Reset the jump count when landed
                    break;
            }
        }

        private IEnumerator playerDash() {
            isDashing = true;
            float originalSpeed = maxSpeed;
            maxSpeed = dashSpeed;
            dashTime = Time.time + dashDuration;
            dashCooldownTime = Time.time + dashCooldown;

            while (Time.time < dashTime)
            {
                move.x = Input.GetAxis("Horizontal");
                yield return null;
            }

            maxSpeed = originalSpeed;
            isDashing = false;
        }

        protected override void ComputeVelocity()
        {
            if (jump)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (!isDashing) {
                if (move.x > 0.01f)
                    spriteRenderer.flipX = false;
                else if (move.x < -0.01f)
                    spriteRenderer.flipX = true;

                animator.SetBool("grounded", IsGrounded);
                animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            }

            targetVelocity = move * maxSpeed;
        }
        

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("DeathZone"))
            {
                HandlePlayerDeath();
            }

            if (other.gameObject.CompareTag("Victory"))
            {
                scoreManager.CompleteLevel();
            }
        }

        void HandlePlayerDeath()
        {
            controlEnabled = false;  // Disable player control
            animator.SetBool("dead", true);  // Play death animation
            audioSource.PlayOneShot(ouchAudio);  // Play death sound
            scoreManager.CompleteLevel();  // Show victory screen (or handle as needed)
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}
