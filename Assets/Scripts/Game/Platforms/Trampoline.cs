using Game.Enemies;
using UnityEngine;

namespace Game.Platforms
{
    public class Trampoline : MonoBehaviour, IPlatform
    {
        public float trampolineForce;
        public PlatformType platformType;
        public Sprite trampolineOnWait;
        public Sprite trampolineLaunch;

        private SpriteRenderer _spriteRenderer;
        private float _timer = 0.0f;


        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {
            if (_timer > 0.0f)
            {
                _timer -= Time.deltaTime;
            }
            else if (_timer < 0.0f)
            {
                _timer = 0.0f;
                OnWaitTrampoline();
            }
        }


        public PlatformType GetPlatformType()
        {
            return platformType;
        }

        private void OnWaitTrampoline()
        {
            _spriteRenderer.sprite = trampolineOnWait;
        }

        private void LaunchTrampoline()
        {
            _spriteRenderer.sprite = trampolineLaunch;
            _timer = 0.5f;
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            var obj = other.gameObject.GetComponent<IMovable>();
            if (obj is null) return;
            obj.Jump(trampolineForce);
            LaunchTrampoline();
        }
    }
}