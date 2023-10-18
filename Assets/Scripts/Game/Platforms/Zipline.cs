using Animations;
using Game.Actions;
using JetBrains.Annotations;
using UnityEngine;
using AnimationState = Animations.AnimationState;

namespace Game.Platforms
{
    //  TODO устанавливать стартовую позицию при смерти игрока.
    public class Zipline : FixedWalkAction, IPlatform
    {
        public PlatformType platformType;

        private AnimationSprite _animationSprite;
        [CanBeNull] private HeroController _stuck = null;

        private CircleCollider2D _circleCollider2D;

        private float _hideColliderTime = 0.0f;

        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _animationSprite = GetComponent<AnimationSprite>();
        }


        private void FixedUpdate()
        {
            if (_stuck)
            {
                Move();
                _stuck.direction = direction;
                GetOffMe();
            }

            _circleCollider2D.enabled = (_hideColliderTime -= Time.deltaTime) < 0.0f;
        }


        public void RestorePosition()
        {
            transform.position = _startPosition;
        }

        private void GetOffMe()
        {
            var dir = Input.GetAxis("Vertical");
            if (dir < 0.0f)
            {
                _hideColliderTime = 0.5f;
                if (_stuck is not null && _stuck.gameObject.activeInHierarchy)
                {
                    _animationSprite.SetState(AnimationState.Idle);
                    _stuck.transform.SetParent(null);
                    _stuck.ZiplineDetached();
                    _stuck = null;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("ZiplinePoint")) return;
            var heroController = other.gameObject.GetComponent<HeroZiplinePoint>().parent;
            if (heroController is null) return;
            heroController.gameObject.transform.SetParent(transform);
            heroController.ZiplineAttached();
            _animationSprite.SetState(AnimationState.Zipline);
            _stuck = heroController;
        }


        public PlatformType GetPlatformType()
        {
            return platformType;
        }
    }
}