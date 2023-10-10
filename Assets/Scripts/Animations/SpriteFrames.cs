using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animations
{
    [Serializable]
    public class SpriteFrames
    {
        [Tooltip("Animation state")] [SerializeField]
        public AnimationState name;

        [SerializeField] public bool isPlayBackward = false;

        [Tooltip("Animation duration between frames")] [SerializeField]
        public float duration = 0.0f;

        [Tooltip("Start frame")] [SerializeField]
        public int frame = 0;

        [Header("Min and max start time")]
        [Tooltip("If values is 0.0f then frames will loop infinitely")]
        [SerializeField]
        public Vector2 minMaxStartTime;

        private float _nextStartTime = 0.0f;

        [Tooltip("Sprite frames")] [SerializeField]
        public Sprite[] frames;


        private float _timer = 0.0f;
        [HideInInspector] public bool isPlaying = true;


        public void Update(SpriteRenderer spriteRenderer)
        {
            if ((_nextStartTime -= Time.deltaTime) <= 0.0f) isPlaying = true;
            if (!isPlaying) return;
            if (isPlayBackward) PlayBackward(spriteRenderer);
            else PlayForward(spriteRenderer);
        }

        private void PlayForward(SpriteRenderer spriteRenderer)
        {
            _timer += Time.deltaTime;
            if (!(_timer >= duration)) return;
            _timer = 0.0f;
            SetFrame(spriteRenderer, frame);
            NextFrame();
        }

        private void PlayBackward(SpriteRenderer spriteRenderer)
        {
            _timer -= Time.deltaTime;
            if (!(_timer <= -duration)) return;
            _timer = 0.0f;
            SetFrame(spriteRenderer, frame);
            BackFrame();
        }


        private void NextFrame()
        {
            if (++frame >= frames.Length)
            {
                if (minMaxStartTime != Vector2.zero)
                {
                    isPlaying = false;
                    _nextStartTime = Random.Range(minMaxStartTime.x, minMaxStartTime.y);
                }

                ResetFrame();
            }
        }

        private void BackFrame()
        {
            if (--frame < 0)
            {
                if (minMaxStartTime != Vector2.zero)
                {
                    isPlaying = false;
                    _nextStartTime = Random.Range(minMaxStartTime.x, minMaxStartTime.y);
                }

                ResetFrame();
            }
        }

        public void SetFrame(SpriteRenderer spriteRenderer, int currentFrame)
        {
            spriteRenderer.sprite = frames[currentFrame];
        }

        public void ResetFrame()
        {
            frame = isPlayBackward ? frames.Length - 1 : 0;
        }
    }
}