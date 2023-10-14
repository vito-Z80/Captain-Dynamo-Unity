using Animations;
using Game.Actions;
using Game.Drops;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : FixedWalkAction, IDeath
    {
        private AnimationSprite _animationSprite;
        private bool _isDead = false;
        private float _time = 0.0f;
        private float _destroyDelta;

        private float _verticalVelocity = 0.0f;
        private float _gravity = 192.0f;

        public GameData gameData;
        
        private ScoreDisplay _scoreDisplay;
        private ItemPoints _itemPoints;
        
        private void Awake()
        {
            step = Vector3.right / 2.0f;
            _animationSprite = GetComponent<AnimationSprite>();
            _scoreDisplay = GetComponent<ScoreDisplay>();
            _itemPoints = GetComponent<ItemPoints>();
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
            _verticalVelocity -= _gravity * Time.deltaTime ;
            transform.Translate(Vector3.up * (_verticalVelocity * Time.deltaTime));
            _time += Time.deltaTime;
            if (transform.position.y < _destroyDelta)
            {
                Destroy(gameObject);
            }
        }
        
        

        public void ToKill()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AnimationSprite>().enabled = false;
            _destroyDelta = transform.position.y - 128.0f;
            _verticalVelocity = Mathf.Sqrt(2 * 48.0f * _gravity);
            _isDead = true;
            _scoreDisplay.Show();
            gameData.CollectScores(_itemPoints.itemScores);
        }
        
    }
}