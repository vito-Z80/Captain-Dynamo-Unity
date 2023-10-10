using System.Collections.Generic;
using System.Linq;
using Animations;
using Camera;
using Game.Actions;
using Game.Enemies;
using Game.Platforms;
using UnityEngine;
using AnimationState = Animations.AnimationState;

namespace Game
{
    public class HeroController : MonoBehaviour, IMovable
    {
        [HideInInspector] public AnimationSprite animationSprite;
        public Rigidbody2D _rb;
        private BoxCollider2D _bc;
        private AnimationState _animationState;
        private bool _isJumping = false;
        [HideInInspector] public float direction = 0.0f;
        private bool _onZipline = false;
        private bool _isDead = false;


        private const float JumpForce = 50f;
        private Vector2 _additionalSpeedFactor = Vector2.zero;
        private Vector3 _afterDeadPosition = Vector3.zero; //  позиция достигнув которою труп игрока вызывает respawn

        [HideInInspector] public bool isActive = false;

        //
        public CameraController camera;
        public GameObject respawnPoints;
        public TeleportController teleport;
        public float speed = 60.0f;


        private List<Vector3> _respawnPointsList = new List<Vector3>();

        private void Awake()
        {
            _animationState = AnimationState.Idle;
            _rb = GetComponent<Rigidbody2D>();
            _bc = GetComponent<BoxCollider2D>();
            animationSprite = GetComponent<AnimationSprite>();
            _respawnPointsList = respawnPoints.GetComponentsInChildren<Transform>().Select(t => t.position)
                .OrderBy(pos => pos.y).ToList();
            _respawnPointsList.Remove(respawnPoints.transform.position);
            respawnPoints.SetActive(false);
            Respawn();
            teleport.StartLevel(this);
        }

        private void Update()
        {
            if (_isDead || !isActive) return;
            ControlledJump(JumpForce);
            Animation();
            LevelCompleted();
        }

        private void LevelCompleted()
        {
            if (teleport.OnLevelCompleted(this))
            {
                isActive = false;
                transform.position = teleport.finishArea.center;
                animationSprite.SetState(AnimationState.Idle);
                _rb.velocity = Vector3.zero;
                _rb.MovePosition(teleport.finishArea.center);
                teleport.FinishLevel(this);
            }
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
                Move();
            }
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

            if (_isJumping) _animationState = AnimationState.JumpUp;

            if (_onZipline) _animationState = AnimationState.Zipline;
            if (_isDead) _animationState = AnimationState.Dead;

            if (animationSprite.animationState != _animationState)
            {
                animationSprite.SetState(_animationState);
            }
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
            _rb.velocity = new Vector2(speed * direction, _rb.velocity.y) + _additionalSpeedFactor;
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
            _isJumping = true;
            var value = Mathf.Sqrt(2f * jumpForce * Mathf.Abs(Physics2D.gravity.y) * _rb.gravityScale) * _rb.mass;
            _rb.AddForce((Vector2.up * value), ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isJumping = false;
            }
        }


        private void Dead()
        {
            _isDead = true;
            _rb.velocity = Vector2.zero;
            _bc.enabled = false;
            Jump(64.0f);
            camera.isBlocked = true;
            _animationState = AnimationState.Dead;
            animationSprite.SetState(_animationState);
            _afterDeadPosition = transform.position + Vector3.down * (camera.ppc.refResolutionY / 2f);
            CheckRespawnPosition();
        }

        private void Respawn()
        {
            _rb.velocity = Vector2.zero;
            _animationState = AnimationState.Idle;
            _isDead = false;
            _isJumping = false;
            _bc.enabled = true;
            camera.isBlocked = false;
            camera.horizontal = false;
            camera.vertical = true;
            transform.position = _respawnPointsList[0];
            camera.transform.position = Vector3.back * 10.0f + Vector3.up * transform.position.y;
        }

        private void CheckRespawnPosition()
        {
            if (_respawnPointsList.Count == 1) return;
            if (transform.position.y > _respawnPointsList[1].y) _respawnPointsList.RemoveAt(0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (_rb.velocity.y < 0.0f)
                {
                    other.GetComponent<IDeath>().ToKill();
                    _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                    Jump(24.0f);
                }
                else
                {
                    Dead();
                }
            }

            //
            if (other.gameObject.CompareTag("Deadly"))
            {
                Dead();
            }

            if (other.gameObject.CompareTag("Drop"))
            {
                if (other.name.Contains("Diamond"))
                {
                    Destroy(other.gameObject);
                }

                if (other.name.Contains("Score"))
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}