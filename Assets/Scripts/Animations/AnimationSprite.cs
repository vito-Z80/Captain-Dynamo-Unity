using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Animations
{
    public class AnimationSprite : MonoBehaviour
    {
        [HideInInspector] public SpriteRenderer spriteRenderer;
        public AnimationState animationState;
        public SpriteFrames[] animations;
        private readonly Dictionary<AnimationState, SpriteFrames> _animation = new();

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            foreach (var frames in animations)
            {
                _animation[frames.name] = frames;
            }

            spriteRenderer.sprite = _animation[animationState].frames[_animation[animationState].frame];
        }

        private void Update()
        {
            _animation[animationState].Update(spriteRenderer);
        }


        public void FlipX(bool flip)
        {
            spriteRenderer.flipX = flip;
        }

        public void FlipY(bool flip)
        {
            spriteRenderer.flipY = flip;
        }
        
        public void SetState(AnimationState state)
        {
            animationState = state;
            _animation[animationState].isPlaying = true;
        }

        public void StopAnimation()
        {
            _animation[animationState].isPlaying = false;
        }

        public void PLay(AnimationState state)
        {
            animationState = state;
            _animation[animationState].ResetFrame();
            _animation[animationState].isPlaying = true;
        }


        public SpriteFrames GetAnimation(AnimationState state)
        {
            return animations.FirstOrDefault(anim => anim.name == state);
        }
    }
}