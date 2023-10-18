using System;
using Animations;
using UnityEngine;
using AnimationState = Animations.AnimationState;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class Fume : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private AnimationSprite _animationSprite;
        private CircleCollider2D _circleCollider2D;


        private const float FumeTime = 2.0f;

        private float _fumeTime = 0.0f;

        private void Start()
        {
            _fumeTime = Random.value * FumeTime;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animationSprite = GetComponent<AnimationSprite>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }


        private void FixedUpdate()
        {
            _fumeTime += Time.deltaTime;
            if (_fumeTime >= FumeTime)
            {
                _fumeTime = 0.0f;
                _animationSprite.SetState(_animationSprite.animationState == AnimationState.Idle
                    ? AnimationState.Run
                    : AnimationState.Idle);
            }
            _circleCollider2D.enabled = _animationSprite.animationState == AnimationState.Run;
        }
    }
}