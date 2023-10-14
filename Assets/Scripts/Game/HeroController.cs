﻿using System;
using System.Collections.Generic;
using System.Linq;
using Animations;
using Camera;
using Game.Actions;
using Game.Drops;
using Game.Platforms;
using UnityEngine;
using UnityEngine.Serialization;
using AnimationState = Animations.AnimationState;

namespace Game
{
    public class HeroController : MonoBehaviour, IMovable
    {
        [HideInInspector] public AnimationSprite animationSprite;
        public Rigidbody2D _rb;
        private BoxCollider2D _bc;
        private AnimationState _animationState;
        [FormerlySerializedAs("_isJumping")] public bool isJumping = false;
        [HideInInspector] public float direction = 0.0f;
        private bool _onZipline = false;
        private bool _isDead = false;


        private const float JumpForce = 50f;
        private Vector2 _additionalSpeedFactor = Vector2.zero;
        private Vector3 _afterDeadPosition = Vector3.zero; //  позиция достигнув которою труп игрока вызывает respawn
        private Vector3 _deathPosition = Vector3.zero; //  позиция смерти для определения респавна.


        private readonly HeroHandler _heroHandler = new HeroHandler();
        //
        

        [HideInInspector] public bool isActive = false;

        //

        // public CameraController _gameCamera;

        // public GameController gameController;


        public CameraController cam;
        
        public LevelController levelController;

        // private GameObject _respawnPoints;
        // private TeleportController _teleport;
        public float speed = 60.0f;


        // private List<Vector3> _respawnPointsList = new List<Vector3>();

        private void Start()
        {
            
            // _cam = gameController.cameraController;
            // _respawnPoints = levelController.respawnPoints;
            // _teleport = levelController.teleportController;
            // _respawnPointsList = _respawnPoints.GetComponentsInChildren<Transform>().Select(t => t.position)
            //     .OrderBy(pos => pos.y).ToList();
            // _respawnPointsList.Remove(_respawnPoints.transform.position);
            // _respawnPoints.SetActive(false);

            _animationState = AnimationState.Idle;
            _rb = GetComponent<Rigidbody2D>();
            _bc = GetComponent<BoxCollider2D>();
            animationSprite = GetComponent<AnimationSprite>();

            Respawn();
            levelController.teleportController.StartLevel(this);
        }

        private void Update()
        {
            if (_isDead || !isActive) return;
            ControlledJump(JumpForce);
            Animation();
            // LevelCompleted();
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

        public void LevelCompleted(Vector3 lastPosition)
        {
            isActive = false;
            animationSprite.SetState(AnimationState.Idle);
            transform.position = lastPosition + Vector3.up * 16.0f;
            _rb.velocity = Vector3.zero;
            _rb.MovePosition(levelController.teleportController.finishArea.center);
            levelController.teleportController.FinishLevel(this);
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