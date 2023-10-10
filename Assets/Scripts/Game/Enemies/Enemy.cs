using Animations;
using Game.Actions;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : FixedWalkAction, IDeath
    {
        private AnimationSprite _animationSprite;
        private bool _isDead = false;
        private float _time = 0.0f;
        private float _destroyDelta;

        private void Awake()
        {
            step = Vector3.right / 2.0f;
            _animationSprite = GetComponent<AnimationSprite>();
        }

        private void FixedUpdate()
        {
            if (_isDead)
            {
                Dead();
            }
            else
            {
                Move();
                FlipX(_animationSprite);
            }
        }

        private void Dead()
        {
            _time += Time.deltaTime;
            transform.position += Vector3.down * _time;
            if (transform.position.y < _destroyDelta)
            {
                Destroy(gameObject);
            }
        }

        public void ToKill()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AnimationSprite>().enabled = false;
            _destroyDelta = transform.position.y - 512.0f;
            _isDead = true;
        }
    }
}