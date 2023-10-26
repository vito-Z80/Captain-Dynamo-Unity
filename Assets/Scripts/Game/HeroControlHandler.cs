using Animations;
using UnityEngine;
using AnimationState = Animations.AnimationState;

namespace Game
{
    public class HeroControlHandler
    {
        public HeroControlHandler(Rigidbody2D rigidBody2D, AnimationSprite animationSprite)
        {
            _rigidBody2D = rigidBody2D;
            _animationSprite = animationSprite;
        }

        private Rigidbody2D _rigidBody2D;
        private AnimationSprite _animationSprite;
        private AnimationState _animationState = AnimationState.Idle;


        public Vector3 PollKeys()
        {
            var controller = Vector3.zero;
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            var z = Input.GetAxis("Jump");
            controller.x = x > 0.0f ? 1.0f : x < 0.0f ? -1.0f : 0.0f;
            controller.y = y > 0.0f ? 1.0f : y < 0.0f ? -1.0f : 0.0f;
            controller.z = z > 0.0f ? 1.0f : z < 0.0f ? -1.0f : 0.0f;

            if (_rigidBody2D.velocity.y != 0.0f && controller.y < 0.0f) controller.y = 0.0f; //  can`t sit if jump
            if (controller.y < 0.0f) controller.x = 0.0f; //  can`t move if sitting
            return controller;
        }

        public void Animation(Vector3 controller, bool onZipline, bool isDead)
        {
            if (onZipline)
            {
                _animationState = AnimationState.Zipline;
                SetAnimation();
                return;
            }
            switch (controller.x)
            {
                case > 0.0f:
                    _animationSprite.FlipX(true);
                    _animationState = AnimationState.Run;
                    break;
                case < 0.0f:
                    _animationSprite.FlipX(false);
                    _animationState = AnimationState.Run;
                    break;
                case 0.0f:
                    _animationState = AnimationState.Idle;
                    break;
            }

            if (controller.y < -0.0f) _animationState = AnimationState.Sit;
            if (_rigidBody2D.velocity.y > 0.0f) _animationState = AnimationState.JumpUp;
            if (_rigidBody2D.velocity.y < 0.0f) _animationState = AnimationState.JumpDown;

            if (isDead) _animationState = AnimationState.Dead;

            SetAnimation();
        }
        
        private void SetAnimation()
        {
            if (_animationSprite.animationState != _animationState)
            {
                _animationSprite.SetState(_animationState);
            }
        }
        
    }
}