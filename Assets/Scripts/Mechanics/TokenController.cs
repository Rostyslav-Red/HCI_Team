using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class animates all token instances in a scene.
    /// This allows a single update call to animate hundreds of sprite 
    /// animations.
    /// If the tokens property is empty, it will automatically find and load 
    /// all token instances in the scene at runtime.
    /// </summary>
    public class TokenController : MonoBehaviour
    {
        [Tooltip("Frames per second at which tokens are animated.")]
        public float frameRate = 12;
        [Tooltip("Instances of tokens which are animated. If empty, token instances are found and loaded at runtime.")]
        public TokenInstance[] tokens;

        float nextFrameTime = 0;

        [ContextMenu("Find All Tokens")]
        void FindAllTokensInScene()
        {
            tokens = UnityEngine.Object.FindObjectsOfType<TokenInstance>();
        }

        void Awake()
        {
            //if tokens are empty, find all instances.
            //if tokens are not empty, they've been added at editor time.
            if (tokens.Length == 0)
                FindAllTokensInScene();
            //Register all tokens so they can work with this controller.
            for (var i = 0; i < tokens.Length; i++)
            {
                tokens[i].tokenIndex = i;
                tokens[i].controller = this;
            }
        }

        void Update()
        {
            if (Time.time >= nextFrameTime)
            {
                foreach (var token in tokens)
                {
                    if (token != null && !token.collected)
                    {
                        token._renderer.sprite = token.sprites[token.frame];
                        token.frame = (token.frame + 1) % token.sprites.Length;
                    }
                }
                nextFrameTime = Time.time + (1f / frameRate);
            }
        }



        public void ResetTokens()
        {
            for (var i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] != null)
                {
                    tokens[i].ResetToken();
                }
            }
        }

    }
}