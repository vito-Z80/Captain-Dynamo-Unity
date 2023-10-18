using Animations;
using Camera;
using Game.Actions;
using UnityEngine;
using UnityEngine.U2D;
using AnimationState = Animations.AnimationState;

namespace Game
{
    public class HeroController : MonoBehaviour, IMovable
    {
        [HideInInspector] public AnimationSprite animationSprite;
        public Rigidbody2D _rb;
        private BoxCollider2D _bc;
        private AnimationState _animationState;
        public bool isJumping = false;
        [HideInInspector] public float direction = 0.0f;
        private bool _onZipline = false;
        private bool _isDead = false;


        private const float JumpForce = 50f;
        private Vector2 _additionalSpeedFactor = Vector2.zero;
        private Vector3 _afterDeadPosition = Vector3.zero; //  позиция достигнув которою труп игрока вызывает respawn
        private Vector3 _deathPosition = Vector3.zero; //  позиция смерти для определения респавна.


        private readonly HeroHandler _heroHandler = new HeroHandler();
        [HideInInspector] public bool isActive = false;

        public CameraController cam;
        public LevelController levelController;

        public float speed = 60.0f;


        private Bounds _stayCollider;
        private Bounds _sitCollider;


        private void Start()
        {
            _animationState = AnimationState.Idle;
            _rb = GetComponent<Rigidbody2D>();
            _bc = GetComponent<BoxCollider2D>();
            animationSprite = GetComponent<AnimationSprite>();
            _stayCollider = new Bounds(new Vector3(0.0f, -1.5f, 0.0f), new Vector3(7.0f, 13.0f, 0.0f));
            _sitCollider = new Bounds(new Vector3(0.0f, -2.5f, 0.0f), new Vector3(7.0f, 11.0f, 0.0f));
            SetStayCollider();
        }

        private void Update()
        {
            if (_isDead || !isActive) return;
            ControlledJump(JumpForce);
            Animation();
        }

        private void FixedUpdate()
        {
            if (!isActive) return;
            if (_isDead)
            {
                if (transform.position.y < _afterDeadPosition.y)
                {
                    Respawn();
                }
            }
            else
            {
                SetStayCollider();
                Move();
                var vert = Input.GetAxis("Vertical");
                if (vert < 0.0f) SitDown();
            }
        }


        private void SitDown()
        {
            if (isJumping) return;
            _animationState = AnimationState.Sit;
            _bc.offset = _sitCollider.center;
            _bc.size = _sitCollider.size;
        }


        private void Animation()
        {
            switch (direction)
            {
                case > 0.0f:
                    animationSprite.FlipX(true);
                    _animationState = AnimationState.Run;
                    break;
                case < 0.0f:
                    animationSprite.FlipX(false);
                    _animationState = AnimationState.Run;
                    break;
                case 0.0f:
                    _animationState = AnimationState.Idle;
                    break;
            }

            if (isJumping) _animationState = AnimationState.JumpUp;

            if (_onZipline) _animationState = AnimationState.Zipline;
            if (_isDead) _animationState = AnimationState.Dead;

            if (animationSprite.animationState != _animationState)
            {
                animationSprite.SetState(_animationState);
            }
        }

        private void SetStayCollider()
        {
            _bc.offset = _stayCollider.center;
            _bc.size = _stayCollider.size;
        }
        
        public void SetStartPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void ControlledJump(float jumpForce)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _rb.velocity.y == 0.0f)
            {
                Jump(jumpForce);
            }
        }

        private void Move()
        {
            if (_onZipline) return;
            direction = Input.GetAxis("Horizontal");
            var actualSpeed = _rb.velocity.y != 0.0f ? 25.0f : speed; 
            _rb.velocity = new Vector2(actualSpeed * direction, _rb.velocity.y) + _additionalSpeedFactor;
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
            var value = Mathf.Sqrt(2f * jumpForce * Mathf.Abs(Physics2D.gravity.y) * _rb.gravityScale) * _rb.mass;
            _rb.AddForce((Vector2.up * value), ForceMode2D.Impulse);
        }


        public void Dead()
        {
            _isDead = true;
            _rb.velocity = Vector2.zero;
            _bc.enabled = false;
            Jump(64.0f);
            cam.isBlocked = true;
            _animationState = AnimationState.Dead;
            animationSprite.SetState(_animationState);
            _deathPosition = transform.position;
            _afterDeadPosition = _deathPosition + Vector3.down * (cam.ppc.refResolutionY / 2f);
        }

        private void Respawn()
        {
            _rb.velocity = Vector2.zero;
            _animationState = AnimationState.Idle;
            _isDead = false;
            isJumping = false;
            _bc.enabled = true;
            cam.isBlocked = false;
            cam.horizontal = false;
            cam.vertical = true;
            transform.position = levelController.GetRespawnPosition(_deathPosition);
            cam.transform.position = Vector3.back * 10.0f + Vector3.up * transform.position.y;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            _heroHandler.OnCollision(this, collision);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _heroHandler.OnTrigger(this, other);
        }
    }
}