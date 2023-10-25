using UnityEngine;
using AnimationState = Animations.AnimationState;

namespace Game
{
    public class HeroControlHandler
    {
        public HeroControlHandler(Rigidbody2D rigidBody2D, BoxCollider2D boxCollider2D)
        {
            _rigidBody2D = rigidBody2D;
            _boxCollider2D = boxCollider2D;
        }

        private Rigidbody2D _rigidBody2D;
        private BoxCollider2D _boxCollider2D;

        public float step = 0.0f;
        public float vert = 0.0f;
        public bool canWalk = false;
        public bool canJump = false;
        public bool canSit = false;



        private int _sitMove = 0;
         
        public Vector2 Update()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            var dir = new Vector2
            {
                x = x > 0.0f ? 1.0f : x < 0.0f ? -1.0f : 0.0f,
                y = y > 0.0f ? 1.0f : y < 0.0f ? -1.0f : 0.0f
            };

            if (dir.x != 0.0f && dir.y < 0.0f)
            {
                
            }
            
            
            

            // var isCrouching = dir.y < 0.0f;
            // var isRunning = dir.x != 0.0f;
            //
            // var wasCrouching = isCrouching;
            // var wasRunning = isRunning;
            //
            //
            // if (dir.x != 0.0f)
            // {
            //     if (isCrouching)
            //     {
            //         // Отменяем приседание и переходим в бег
            //         isCrouching = false;
            //         isRunning = true;
            //     }
            // }
            // else if (dir.y < 0.0f)
            // {
            //     if (isRunning)
            //     {
            //         // Отменяем бег и переходим в приседание
            //         isRunning = false;
            //         isCrouching = true;
            //     }
            // }
            //
            //
            // if (wasRunning && !isRunning) dir.y = 0.0f;
            // if (wasCrouching && !isCrouching) dir.x = 0.0f;

            return dir;


            // animationState = _rigidBody2D.velocity.y switch
            // {
            //     > 0.0f => AnimationState.JumpUp,
            //     < 0.0f => AnimationState.JumpDown,
            //     _ => animationState
            // };
        }
    }
}