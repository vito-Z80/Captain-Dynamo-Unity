using System;
using Animations;
using Behaviors;
using JetBrains.Annotations;
using TileMap;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Enemies
{
    public class HorizontalWalkEnemy : MonoBehaviour
    {
        private Tilemap _tilemap;
        private TileCollision _tiles;
        private AutoWalk _autoWalk;
        private BoxCollider2D _boxCollider2D;
        private AnimationSprite _animationSprite;

        private HorizontalDirection _hDirection;
        // private VerticalDirection _vDirection;
        private float _speed = 1.0f;
        [HideInInspector] [ItemCanBeNull] public TileBase[] tiles = new TileBase[4];

        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            _tiles = GetComponent<TileCollision>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _animationSprite = GetComponent<AnimationSprite>();
            _autoWalk = GetComponent<AutoWalk>();
        }


        private void Update()
        {
            _tiles.GetAllTiles(_tilemap, _boxCollider2D.bounds, tiles);
            _hDirection = _autoWalk.GetHorizontalDirection(tiles, Define.Wall);
            Move();
        }


        private void Move()
        {
            switch (_hDirection)
            {
                case HorizontalDirection.Left:
                    transform.position += Vector3.left * _speed;
                    _animationSprite.spriteRenderer.flipX = true;
                    break;
                case HorizontalDirection.Right:
                    transform.position += Vector3.right * _speed;
                    _animationSprite.spriteRenderer.flipX = false;
                    break;
                case HorizontalDirection.Stay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}