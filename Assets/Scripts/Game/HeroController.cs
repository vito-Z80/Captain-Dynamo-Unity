using System;
using Animations;
using Camera;
using Game.Actions;
using Game.GameMenu;
using UnityEngine;
using AnimationState = Animations.AnimationState;

namespace Game
{
    public class HeroController : MonoBehaviour, IMovable
    {
        public AudioSource jumpSound;
        public AudioSource diamondSound;
        public AudioSource deathSound;
        public AudioSource killSound;


        [HideInInspector] public AnimationSprite animationSprite;
        [HideInInspector] public Rigidbody2D _rb;
        private BoxCollider2D _bc;
        // private AnimationState _animationState;
        [HideInInspector] public bool isJumping = false;
        [HideInInspector] public float direction = 0.0f;
        private bool _onZipline = false;
        private bool _isDead = false;
        // private bool _isSitting = false;


        private const float HighJumpForce = 50f;
        private const float LowJumpForce = 38f;

        private Vector2 _additionalSpeedFactor = Vector2.zero;

        private Vector3 _maxHeroPosition;


        private readonly HeroHandler _heroHandler = new HeroHandler();

        private bool _isActive = false;

        public CameraController cam;
        public LevelController levelController;

        public float speed = 60.0f;

        [HideInInspector] public float assignedSpeed;

        private Bounds _stayCollider;
        private Bounds _sitCollider;


        private HeroControlHandler _heroControlHandler;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _bc = GetComponent<BoxCollider2D>();
            animationSprite = GetComponent<AnimationSprite>();
            _stayCollider = new Bounds(new Vector3(0.0f, -1.5f, 0.0f), new Vector3(7.0f, 13.0f, 0.0f));
            _sitCollider = new Bounds(new Vector3(0.0f, -2.5f, 0.0f), new Vector3(7.0f, 11.0f, 0.0f));
            SetStayCollider();
            assignedSpeed = speed;
            _maxHeroPosition = Vector3.zero + Vector3.down * 2048.0f;
            _heroControlHandler = new HeroControlHandler(_rb, animationSprite);
        }

        private void Update()
        {
            Respawn();
            if (_isDead || !_isActive) return;
            var controller = _heroControlHandler.PollKeys();
            MoveHero(controller);
            JumpHero(controller);
            _heroControlHandler.Animation(controller, _onZipline, _isDead);
            var heroPos = transform.position;
            _maxHeroPosition = new Vector3(heroPos.x, Mathf.Max(heroPos.y, _maxHeroPosition.y), heroPos.z);
        }

        private void SetSitCollider()
        {
            _bc.offset = _sitCollider.center;
            _bc.size = _sitCollider.size;
        }


        private void SetStayCollider()
        {
            _bc.offset = _stayCollider.center;
            _bc.size = _stayCollider.size;
        }

        public void SetStayPosition(Vector3 position)
        {
            transform.position = position;
            direction = 0.0f;
            animationSprite.SetState(AnimationState.Idle);
        }

        private void JumpHero(Vector3 controller)
        {
            if (controller.z > 0.0f && _rb.velocity.y == 0.0f && !_onZipline)
            {
                if (controller.y > 0.0f) Jump(HighJumpForce);
                else Jump(LowJumpForce);
            }
        }

        private void MoveHero(Vector3 controller)
        {
            if (_onZipline) return;
            if (controller.y < 0.0f)
            {
                _rb.velocity = Vector2.zero;
                SetSitCollider();
                return;
            }

            SetStayCollider();
            var actualSpeed = _rb.velocity.y != 0.0f ? 25.0f : speed; //  В прыжке уменьшить горизонтальное перемещение.
            _rb.velocity = new Vector2(actualSpeed * controller.x, _rb.velocity.y) + _additionalSpeedFactor;
        }

        public void SetAdditionalSpeed(Vector2 additionalSpeedFactor)
        {
            _additionalSpeedFactor = additionalSpeedFactor * speed;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }


        public void ZiplineAttached()
        {
            _onZipline = true;
        }

        public void ZiplineDetached()
        {
            _onZipline = false;
        }

        public void Jump(float jumpForce)
        {
            isJumping = true;
            jumpSound.Play();
            var value = Mathf.Sqrt(2f * jumpForce * Mathf.Abs(Physics2D.gravity.y) * _rb.gravityScale) * _rb.mass;
            _rb.AddForce((Vector2.up * value), ForceMode2D.Impulse);
        }


        public void Dead()
        {
            deathSound.Play();
            levelController.RestoreZiplines();
            _isDead = true;
            _rb.velocity = Vector2.zero;
            _bc.enabled = false;
            Jump(64.0f);
            cam.isBlocked = true;
            animationSprite.SetState(AnimationState.Dead);
            direction = 0;
            levelController.gameData.deadCount++;
        }

        private void Respawn()
        {
            if (transform.position.y > cam.transform.position.y - cam.ppc.refResolutionY) return;
            _rb.velocity = Vector2.zero;
            // _animationState = AnimationState.Idle;
            direction = 0;
            _isDead = false;
            isJumping = false;
            _bc.enabled = true;
            cam.isBlocked = false;
            cam.horizontal = false;
            cam.vertical = true;
            transform.position = levelController.GetRespawnPosition(_maxHeroPosition);
            cam.transform.position = Vector3.back * 10.0f + Vector3.up * transform.position.y;
        }

        public void ActiveState(bool isActivate)
        {
            // isActivate ^= levelController.gameData.isGameMenuShowing;
            _rb.simulated = isActivate;
            _bc.enabled = isActivate;
            _isActive = isActivate;
        }

        public bool IsActive() => _isActive;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _heroHandler.OnCollision(this, collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _heroHandler.OnCollisionExit(this, collision);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _heroHandler.OnTrigger(this, other);
        }
    }
}