using System;
using UnityEngine;

namespace Game.Platforms
{
    public class PlatformMoveArea
    {
        private readonly float _speed;
        private VerticalDirection _vDirection = VerticalDirection.Down;
        private HorizontalDirection _hDirection = HorizontalDirection.Right;

        private readonly Vector3 _min;
        private readonly Vector3 _max;
        private readonly Transform _transform;

        public PlatformMoveArea(Transform transform, Vector3 minPoint, Vector3 maxPoint, float speed)
        {
            _speed = speed;
            _transform = transform;

            if (minPoint.x < maxPoint.x)
            {
                _min.x = minPoint.x;
                _max.x = maxPoint.x;
            }
            else
            {
                _min.x = maxPoint.x;
                _max.x = minPoint.x;
            }

            if (minPoint.y < maxPoint.y)
            {
                _min.y = minPoint.y;
                _max.y = maxPoint.y;
            }
            else
            {
                _min.y = maxPoint.y;
                _max.y = minPoint.y;
            }

            _min += transform.position;
            _max += transform.position;
        }


        public void Move()
        {
            var pos = _transform.position;
            var delta = Time.deltaTime;
            switch (_vDirection)
            {
                case VerticalDirection.Up:
                    pos.y += _speed * delta;
                    break;
                case VerticalDirection.Down:
                    pos.y -= _speed * delta;
                    break;
                case VerticalDirection.Stay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (_hDirection)
            {
                case HorizontalDirection.Left:
                    pos.x -= _speed * delta;
                    break;
                case HorizontalDirection.Right:
                    pos.x += _speed * delta;
                    break;

                case HorizontalDirection.Stay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (pos.y < _min.y)
            {
                _vDirection = VerticalDirection.Up;
                pos.y += _speed * delta;
            }

            if (pos.y > _max.y)
            {
                _vDirection = VerticalDirection.Down;
                pos.y -= _speed * delta;
            }

            if (pos.x < _min.x)
            {
                _hDirection = HorizontalDirection.Right;
                pos.x += _speed * delta;
            }

            if (pos.x > _max.x)
            {
                _hDirection = HorizontalDirection.Left;
                pos.x -= _speed * delta;
            }


            _transform.position = pos;
        }
    }
}